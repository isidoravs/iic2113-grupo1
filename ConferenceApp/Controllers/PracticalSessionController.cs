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
    public class PracticalSessionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PracticalSessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PracticalSession
        public async Task<IActionResult> Index()
        {
            return View(await _context.PracticalSessions.ToListAsync());
        }

        // GET: PracticalSession/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practicalSession = await _context.PracticalSessions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (practicalSession == null)
            {
                return NotFound();
            }
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAssistant = await _context.Roles.Where(x => (x.UserId == currentUserId && x.EventId == practicalSession.Id)).ToListAsync();

            int assisting = isAssistant.Count;
            ViewBag.assisting = assisting;

            var tagNames = new List<String>();
            var joinedTags = "";
            var eventTags = await _context.EventTags.Where(x => (x.EventId == practicalSession.Id)).ToListAsync();
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

            var room = await _context.Rooms.FindAsync(@practicalSession.RoomId);
            var centre = await _context.EventCentres.FindAsync(room.EventCentreId);
            var version = await _context.ConferenceVersions.FindAsync(@practicalSession.ConferenceVersionId);
            var conference = await _context.Conferences.FindAsync(version.ConferenceId);

            var sponsorships = await _context.Sponsorships.Where(x => x.ConferenceVersionId == version.Id).ToListAsync();
            var sponsors = new List<object>();
            foreach (var member in sponsorships)
            {
                var s = await _context.Sponsors.FindAsync(member.SponsorId);
                sponsors.Add(s.Name);
            }

            var assistantRoles = await _context.Roles.Where(x => x.EventId == @practicalSession.Id).ToListAsync();
            var assistants = new List<object>();
            // foreach (var member in assistantRoles)
            // {
            //     var a = await _context.Users.FindAsync(member.UserId);
            //     assistants.Add(a.Email);
            // }

            ViewBag.roomName = room.Name;
            ViewBag.centreName = centre.Name;
            ViewBag.location = centre.Location;
            ViewBag.version = version;
            ViewBag.conference = conference;
            ViewBag.assistants = assistants;
            ViewBag.sponsors = sponsors;

            return View(practicalSession);
        }

        // GET: PracticalSession/Create
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

        // POST: PracticalSession/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Topic,Id,Name,StartDate,EndDate,ConferenceVersionId,RoomId,Exhibitor,AvailableTags")] PracticalSession practicalSession)
        {
        //    var conferenceVersion = await _context.ConferenceVersions.Where(x => x.Id == practicalSession.ConferenceVersionId).FirstOrDefaultAsync();
        //    if (conferenceVersion.StartDate > practicalSession.StartDate || conferenceVersion.EndDate < practicalSession.EndDate)
        //    {
        //        // hay problemas con la fecha
        //        TempData["DateError"] = "Valor temporal";
        //    }
        //    else
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(practicalSession);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Details), new { id = practicalSession.Id.ToString() });

        //        }
        //    }
        //    return RedirectToAction("Index", "Event");
        //}
        var conferenceVersion = await _context.ConferenceVersions.Where(x => x.Id == practicalSession.ConferenceVersionId).FirstOrDefaultAsync();
        var events = await _context.Events.Where(x => x.ConferenceVersionId == conferenceVersion.Id).ToListAsync();
        var room = await _context.Rooms.Where(x => x.Id == practicalSession.RoomId).FirstOrDefaultAsync();
        var isOccupied = 0;

        var sharedRoomEvents = await _context.Events.Where(x => x.ConferenceVersionId == practicalSession.ConferenceVersionId && x.RoomId == practicalSession.RoomId).ToListAsync();
            foreach (var even in sharedRoomEvents)
            {
                if (practicalSession.StartDate <= even.StartDate && practicalSession.EndDate >= even.StartDate)
                {
                    isOccupied = 1;
                }
                else if (practicalSession.StartDate <= even.EndDate && practicalSession.EndDate >= even.EndDate)
                {
                    isOccupied = 1;
                }
                else if (practicalSession.StartDate >= even.StartDate && practicalSession.EndDate <= even.EndDate)
                {
                    isOccupied = 1;
                }
                else if (practicalSession.StartDate <= even.StartDate && practicalSession.EndDate >= even.EndDate)
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
                if (conferenceVersion.StartDate > practicalSession.StartDate || conferenceVersion.EndDate<practicalSession.EndDate)
                {
                    // hay problemas con la fecha
                    TempData["DateError"] = "Valor temporal";
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        var eventTags = new List<EventTag>();
                        for (var i = 0; i < practicalSession.AvailableTags.Count; i++)
                        {
                            if (practicalSession.AvailableTags[i].IsChecked)
                            {
                                var tag = await _context.Tags.FirstOrDefaultAsync(m => m.Id == practicalSession.AvailableTags[i].TagId);
                                var eventTag = new EventTag() {Event = practicalSession, EventId = practicalSession.Id, Tag = tag, TagId = tag.Id};
                                _context.Add(eventTag);
                                eventTags.Add(eventTag);
                            }
                        }
                        practicalSession.EventTags = eventTags;
                        _context.Add(practicalSession);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Details), new { id = practicalSession.Id.ToString() });
                    }
                }
            }
            return RedirectToAction("Index", "Event");
        }

        // GET: PracticalSession/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practicalSession = await _context.PracticalSessions.FindAsync(id);
            if (practicalSession == null)
            {
                return NotFound();
            }
            var conferenceVersion = await _context.ConferenceVersions.Where(x => x.Id == practicalSession.ConferenceVersionId).FirstOrDefaultAsync();
            var rooms = await _context.Rooms.Where(x => x.EventCentreId == conferenceVersion.EventCentreId).ToListAsync();
            this.ViewData["Rooms"] = new SelectList(rooms, "Id", "Name");

            var tags = await _context.Tags.ToListAsync();
            //var eventTags = await _context.EventTags.Where(x => (x.EventId == chat.Id)).ToListAsync();
            var availableTags = new List<CheckBoxItem>();
            foreach (var tag in tags)
            {
                var eventTag = await _context.EventTags.FirstOrDefaultAsync(x => x.EventId == practicalSession.Id && x.TagId == tag.Id);
                var checkBox = new CheckBoxItem() {TagId = tag.Id, Title = tag.Name, IsChecked = eventTag != null};
                availableTags.Add(checkBox);
            }
            this.ViewData["AvailableTags"] = availableTags;
            return View(practicalSession);
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
                    TempData["AssistError"] = "Valor Temporal";
                    break;
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

        // POST: PracticalSession/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Topic,Id,Name,StartDate,EndDate,ConferenceVersionId,FileId,RoomId,Exhibitor,AvailableTags")] PracticalSession practicalSession)
        {
            if (id != practicalSession.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var eventTags = new List<EventTag>();
                    for (var i = 0; i < practicalSession.AvailableTags.Count; i++)
                    {
                        var existingEventTag = await _context.EventTags.FirstOrDefaultAsync(x => x.TagId == practicalSession.AvailableTags[i].TagId && x.EventId == practicalSession.Id);
                        if (practicalSession.AvailableTags[i].IsChecked)
                        {
                            // si el tag está checkeado y ya exitía este eventTag, no hacer nada, sino crearlo
                            if (existingEventTag == null)
                            {
                                var tag = await _context.Tags.FirstOrDefaultAsync(x => x.Id == practicalSession.AvailableTags[i].TagId);
                                var newEventTag = new EventTag() {Event = practicalSession, EventId = practicalSession.Id, Tag = tag, TagId = tag.Id};
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
                    practicalSession.EventTags = eventTags;  // actualizamos los eventTags del chat
                    _context.Update(practicalSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PracticalSessionExists(practicalSession.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = practicalSession.Id.ToString() });
            }
            return View(practicalSession);
        }

        // GET: PracticalSession/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practicalSession = await _context.PracticalSessions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (practicalSession == null)
            {
                return NotFound();
            }

            return View(practicalSession);
        }

        // POST: PracticalSession/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var practicalSession = await _context.PracticalSessions.FindAsync(id);
            _context.PracticalSessions.Remove(practicalSession);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PracticalSessionExists(int id)
        {
            return _context.PracticalSessions.Any(e => e.Id == id);
        }
    }
}
