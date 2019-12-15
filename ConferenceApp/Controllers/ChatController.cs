using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConferenceApp.Data;
using ConferenceApp.Models;

namespace ConferenceApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Chat
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chats.ToListAsync());
        }

        // GET: Chat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAssistant = await _context.Roles.Where(x => (x.UserId == currentUserId && x.EventId == chat.Id)).ToListAsync();

            int assisting = isAssistant.Count;
            ViewBag.assisting = assisting;

            var tagNames = new List<String>();
            var joinedTags = "";
            var eventTags = await _context.EventTags.Where(x => (x.EventId == chat.Id)).ToListAsync();
            if (eventTags.Count > 0)
            {
                foreach (var et in eventTags)
                {
                    var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == et.TagId);
                    tagNames.Add("<button type='button' class='btn btn-outline-secondary'>"+tag.Name+"</button>");
                }
                joinedTags = String.Join(" ", tagNames);
            }
            ViewBag.joinedTags = joinedTags;

            var room = await _context.Rooms.FindAsync(@chat.RoomId);
            var centre = await _context.EventCentres.FindAsync(room.EventCentreId);
            var version = await _context.ConferenceVersions.FindAsync(@chat.ConferenceVersionId);
            var conference = await _context.Conferences.FindAsync(version.ConferenceId);

            var sponsorships = await _context.Sponsorships.Where(x => x.ConferenceVersionId == version.Id).ToListAsync();
            var sponsors = new List<object>();
            foreach (var member in sponsorships)
            {
                var s = await _context.Sponsors.FindAsync(member.SponsorId);
                sponsors.Add(s.Name);
            }

            var assistantRoles = await _context.Roles.Where(x => x.EventId == @chat.Id).ToListAsync();
            var assistants = new List<object>();

            // foreach (var member in assistantRoles)
            // {
            //     var a = await _context.Users.FindAsync(member.UserId);
            //     assistants.Add(a.Email);
            // }

            var EventAssistance = await _context.Roles.Where(x => x.EventId == @chat.Id).CountAsync();

            ViewBag.roomName = room.Name;
            ViewBag.centreName = centre.Name;
            ViewBag.location = centre.Location;
            ViewBag.version = version;
            ViewBag.conference = conference;
            ViewBag.assistants = assistants;
            ViewBag.sponsors = sponsors;
            ViewBag.EventAssistance = EventAssistance;

            return View(chat);
        }

        // GET: Chat/Create
        public async Task<IActionResult> Create(int? conferenceVersionId)
        {
            var conferenceVersions = conferenceVersionId == null
                ? await _context.ConferenceVersions.ToListAsync()
                : await _context.ConferenceVersions.Where(x => x.Id == conferenceVersionId).ToListAsync();

            var rooms = await _context.Rooms.Where(x => x.EventCentreId == conferenceVersions[0].EventCentreId).ToListAsync();

            List<object> versions = new List<object>();
            foreach (var member in conferenceVersions)
                versions.Add( new {
                    Id = member.Id,
                    Name = (await _context.Conferences.FindAsync(member.ConferenceId)).Name + " (versión " + member.Number + ")"
                } );

            var tags = await _context.Tags.ToListAsync();
            var availableTags = tags.Select(tag => new CheckBoxItem() {TagId = tag.Id, Title = tag.Name, IsChecked = false}).ToList();

            this.ViewData["ConferenceVersions"] = new SelectList(versions, "Id", "Name");
            this.ViewData["Rooms"] = new SelectList(rooms, "Id", "Name");
            this.ViewData["AvailableTags"] = new List<CheckBoxItem>(availableTags);
            return View();
        }

        public async Task<IActionResult> RemoveAssistant(int eventId)
        {

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var assistants = await _context.Roles.Where(x => (x.UserId == currentUserId && x.EventId == eventId)).ToListAsync();
            _context.Roles.RemoveRange(assistants);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = eventId.ToString() });

        }
        public async Task<IActionResult> AddAssistant(int eventId)
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var @thisEvent = await _context.Events.FirstOrDefaultAsync(m => m.Id == eventId);
            var isOccupied = 0;
            
            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == @thisEvent.RoomId);
            var capacityUsed = await _context.Roles.Where(x => (x.EventId == eventId && x.Name == "attendant")).ToListAsync();

            if (room.MaxCapacity <= capacityUsed.Count)
            {
                TempData["AssistError"] = "Se ha alcanzado la capacidad máxima para este evento";
                isOccupied = 1;
            }
            else
            {
                var assistingToEvents = await _context.Roles.Where(x => (x.UserId == currentUserId)).ToListAsync();
                foreach (var aRole in assistingToEvents)
                {
                    var @event = await _context.Events.FirstOrDefaultAsync(m => m.Id == aRole.EventId);
                    if (@event.StartDate <= @thisEvent.StartDate && @event.EndDate >= @thisEvent.StartDate )
                    {
                        isOccupied = 1;
                    }
                    else if (@event.StartDate <= @thisEvent.EndDate && @event.EndDate >= @thisEvent.EndDate )
                    {
                        isOccupied = 1;
                    }
                    else if (@event.StartDate >= @thisEvent.StartDate && @event.StartDate <= @thisEvent.EndDate )
                    {
                        isOccupied = 1;
                    }
                    else if (@event.EndDate >= @thisEvent.StartDate && @event.EndDate <= @thisEvent.EndDate )
                    {
                        isOccupied = 1;
                    }
                    if (isOccupied == 1)
                    {
                        TempData["AssistError"] = "Tienes tope de horario con este evento";
                        break;
                    }
                }
            }
            
            if (isOccupied == 0)
            {
                var role = new Role() {UserId = currentUserId, EventId = eventId};
                _context.Add(role);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id = eventId.ToString() });

        }

        // POST: Chat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Topic,Id,Name,StartDate,EndDate,ConferenceVersionId,RoomId,Panelists,Moderator,AvailableTags")] Chat chat)
        {
            var conferenceVersion = await _context.ConferenceVersions.Where(x => x.Id == chat.ConferenceVersionId).FirstOrDefaultAsync();
            var events = await _context.Events.Where(x => x.ConferenceVersionId == conferenceVersion.Id).ToListAsync();
            var room = await _context.Rooms.Where(x => x.Id == chat.RoomId).FirstOrDefaultAsync();
            var isOccupied = 0;

            var sharedRoomEvents = await _context.Events.Where(x => x.ConferenceVersionId == chat.ConferenceVersionId && x.RoomId == chat.RoomId).ToListAsync();
            foreach (var even in sharedRoomEvents)
            {
                if (chat.StartDate <= even.StartDate && chat.EndDate >= even.StartDate)
                {
                    isOccupied = 1;
                }
                else if (chat.StartDate <= even.EndDate && chat.EndDate >= even.EndDate)
                {
                    isOccupied = 1;
                }
                else if (chat.StartDate >= even.StartDate && chat.EndDate <= even.EndDate)
                {
                    isOccupied = 1;
                }
                else if (chat.StartDate <= even.StartDate && chat.EndDate >= even.EndDate)
                {
                    isOccupied = 1;
                }
                if (isOccupied == 1)
                {
                    TempData["RoomError"] = "Valor Temporal";
                    break;
                }
            }
            if (isOccupied == 0)
            {
                if (conferenceVersion.StartDate > chat.StartDate || conferenceVersion.EndDate < chat.EndDate)
                {
                    // hay problemas con la fecha
                    TempData["DateError"] = "Valor temporal";
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var eventTags = new List<EventTag>();
                        if (chat.AvailableTags != null)
                        {
                            for (var i = 0; i < chat.AvailableTags.Count; i++)
                            {
                                if (chat.AvailableTags[i].IsChecked)
                                {
                                    var tag = await _context.Tags.FirstOrDefaultAsync(m => m.Id == chat.AvailableTags[i].TagId);
                                    var eventTag = new EventTag() {Event = chat, EventId = chat.Id, Tag = tag, TagId = tag.Id};
                                    _context.Add(eventTag);
                                    eventTags.Add(eventTag);
                                }
                            }
                        }
                        chat.EventTags = eventTags;
                        _context.Add(chat);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Details), new { id = chat.Id.ToString() });
                    }
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var e in errors)
            {
                Console.WriteLine(e.ErrorMessage);
            }

            return RedirectToAction("Index", "Event");
        }

        // GET: Chat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats.FindAsync(id);
            if (chat == null)
            {
                return NotFound();
            }
            var conferenceVersion = await _context.ConferenceVersions.Where(x => x.Id == chat.ConferenceVersionId).FirstOrDefaultAsync();
            var rooms = await _context.Rooms.Where(x => x.EventCentreId == conferenceVersion.EventCentreId).ToListAsync();
            this.ViewData["Rooms"] = new SelectList(rooms, "Id", "Name");

            var tags = await _context.Tags.ToListAsync();
            var availableTags = new List<CheckBoxItem>();
            foreach (var tag in tags)
            {
                var eventTag = await _context.EventTags.FirstOrDefaultAsync(x => x.EventId == chat.Id && x.TagId == tag.Id);
                var checkBox = new CheckBoxItem() {TagId = tag.Id, Title = tag.Name, IsChecked = eventTag != null};
                availableTags.Add(checkBox);
            }
            this.ViewData["AvailableTags"] = availableTags;

            return View(chat);
        }

        // POST: Chat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Topic,Id,Name,StartDate,EndDate,ConferenceVersionId,RoomId,Panelists,Moderator,AvailableTags")] Chat chat)
        {
            if (id != chat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var eventTags = new List<EventTag>();
                    if (chat.AvailableTags != null)
                    {
                        for (var i = 0; i < chat.AvailableTags.Count; i++)
                        {
                            var existingEventTag = await _context.EventTags.FirstOrDefaultAsync(x => x.TagId == chat.AvailableTags[i].TagId && x.EventId == chat.Id);
                            if (chat.AvailableTags[i].IsChecked)
                            {
                                // si el tag está checkeado y ya exitía este eventTag, no hacer nada, sino crearlo
                                if (existingEventTag == null)
                                {
                                    var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == chat.AvailableTags[i].TagId);
                                    var newEventTag = new EventTag() {Event = chat, EventId = chat.Id, Tag = tag, TagId = tag.Id};
                                    _context.Add(newEventTag);
                                    eventTags.Add(newEventTag);
                                }
                            }
                            else
                            {
                                // si no está checkeado y ya exitía este eventTag, eliminarlo, sino no hacer nada
                                if (existingEventTag != null)
                                {
                                    _context.EventTags.Remove(existingEventTag);
                                }
                            }
                        }
                    }
                    chat.EventTags = eventTags;  // actualizamos los eventTags del chat

                    _context.Update(chat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatExists(chat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = chat.Id.ToString() });
            }
            return View(chat);
        }

        // GET: Chat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chat = await _context.Chats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chat == null)
            {
                return NotFound();
            }

            return View(chat);
        }

        // POST: Chat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            _context.Chats.Remove(chat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatExists(int id)
        {
            return _context.Chats.Any(e => e.Id == id);
        }
    }
}
