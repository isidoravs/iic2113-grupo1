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
    public class PartyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Party
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parties.ToListAsync());
        }

        // GET: Party/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Parties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAssistant = await _context.Roles.Where(x => (x.UserId == currentUserId && x.EventId == party.Id && x.Name == "attendant")).ToListAsync();

            int assisting = isAssistant.Count;
            ViewBag.assisting = assisting;

            var room = await _context.Rooms.FindAsync(@party.RoomId);
            var centre = await _context.EventCentres.FindAsync(room.EventCentreId);
            var version = await _context.ConferenceVersions.FindAsync(@party.ConferenceVersionId);
            var conference = await _context.Conferences.FindAsync(version.ConferenceId);

            var sponsorships = await _context.Sponsorships.Where(x => x.ConferenceVersionId == version.Id).ToListAsync();
            var sponsors = new List<object>();
            foreach (var member in sponsorships)
            {
                var s = await _context.Sponsors.FindAsync(member.SponsorId);
                sponsors.Add(s.Name);
            }

            var assistantRoles = await _context.Roles.Where(x => x.EventId == @party.Id && x.Name == "attendant").ToListAsync();
            var assistants = new List<object>();
            // foreach (var member in assistantRoles)
            // {
            //     var a = await _context.Users.FindAsync(member.UserId);
            //     assistants.Add(a.Email);
            // }

            var EventAssistance = await _context.Roles.Where(x => x.EventId == party.Id && x.Name == "attendant").CountAsync();

            var FeedbackCategories = await _context.FeedbackCategories.ToListAsync();
            var Feedbacks = await _context.Feedbacks.Where(x => x.EventId == party.Id).ToListAsync();

            var FeedbackAveragePerCategory = new List<object>();
            var FeedbackCategoryName = new List<object>();

            foreach (var Category in FeedbackCategories)
            {
                if (Category.Name != "Exhibitor")
                {
                    FeedbackCategoryName.Add(Category.Name);
                    var FeedbacksScopesOfEventAndCategory = await _context.FeedbackScopes.Where(fs => Feedbacks.Any(f => fs.FeedbackId == f.Id && fs.FeedbackCategoryId == Category.Id)).ToListAsync();

                    if (FeedbacksScopesOfEventAndCategory.Count() >= 1)
                    {
                        FeedbackAveragePerCategory.Add(FeedbacksScopesOfEventAndCategory.Average(f => f.Grade).ToString());
                    }
                    else
                    {
                        FeedbackAveragePerCategory.Add("No hay evaluaciones todavía");
                    }
                }
            }

            ViewBag.feedback = await _context.Feedbacks.FirstOrDefaultAsync(f => f.UserId == currentUserId);

            ViewBag.roomName = room.Name;
            ViewBag.centreName = centre.Name;
            ViewBag.location = centre.Location;
            ViewBag.version = version;
            ViewBag.conference = conference;
            ViewBag.assistants = assistants;
            ViewBag.sponsors = sponsors;
            ViewBag.EventAssistance = EventAssistance;
            ViewBag.FeedbackCategoryName = FeedbackCategoryName;
            ViewBag.FeedbackAveragePerCategory = FeedbackAveragePerCategory;


            return View(party);
        }

        // GET: Party/Create
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
            this.ViewData["ConferenceVersions"] = new SelectList(versions, "Id", "Name");
            this.ViewData["Rooms"] = new SelectList(rooms, "Id", "Name");
            return View();
        }
        public async Task<IActionResult> RemoveAssistant(int eventId)
        {

            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var assistants = await _context.Roles.Where(x => (x.UserId == currentUserId && x.EventId == eventId && x.Name == "attendant")).ToListAsync();
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
                var assistingToEvents = await _context.Roles.Where(x => (x.UserId == currentUserId && x.Name == "attendant")).ToListAsync();
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
        // POST: Party/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MusicStyle,Id,Name,StartDate,EndDate,ConferenceVersionId,RoomId")] Party party)
        {
            var conferenceVersion = await _context.ConferenceVersions.Where(x => x.Id == party.ConferenceVersionId).FirstOrDefaultAsync();
            var events = await _context.Events.Where(x => x.ConferenceVersionId == conferenceVersion.Id).ToListAsync();
            var room = await _context.Rooms.Where(x => x.Id == party.RoomId).FirstOrDefaultAsync();
            var isOccupied = 0;

            var sharedRoomEvents = await _context.Events.Where(x => x.ConferenceVersionId == party.ConferenceVersionId && x.RoomId == party.RoomId).ToListAsync();
            foreach (var even in sharedRoomEvents)
            {
                if (party.StartDate <= even.StartDate && party.EndDate >= even.StartDate)
                {
                    isOccupied = 1;
                }
                else if (party.StartDate <= even.EndDate && party.EndDate >= even.EndDate)
                {
                    isOccupied = 1;
                }
                else if (party.StartDate >= even.StartDate && party.EndDate <= even.EndDate)
                {
                    isOccupied = 1;
                }
                else if (party.StartDate <= even.StartDate && party.EndDate >= even.EndDate)
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
                if (conferenceVersion.StartDate > party.StartDate || conferenceVersion.EndDate < party.EndDate)
                {
                    // hay problemas con la fecha
                    TempData["DateError"] = "Valor temporal";
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(party);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Details), new { id = party.Id.ToString() });
                    }
                }
            }
            return RedirectToAction("Index", "Event");
        }

        // GET: Party/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Parties.FindAsync(id);
            if (party == null)
            {
                return NotFound();
            }
            var conferenceVersion = await _context.ConferenceVersions.Where(x => x.Id == party.ConferenceVersionId).FirstOrDefaultAsync();
            var rooms = await _context.Rooms.Where(x => x.EventCentreId == conferenceVersion.EventCentreId).ToListAsync();
            this.ViewData["Rooms"] = new SelectList(rooms, "Id", "Name");
            return View(party);
        }

        // POST: Party/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MusicStyle,Id,Name,StartDate,EndDate,ConferenceVersionId,RoomId")] Party party)
        {
            if (id != party.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(party);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartyExists(party.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = party.Id.ToString() });
            }
            return View(party);
        }

        // GET: Party/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Parties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // POST: Party/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            _context.Parties.Remove(party);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartyExists(int id)
        {
            return _context.Parties.Any(e => e.Id == id);
        }
    }
}
