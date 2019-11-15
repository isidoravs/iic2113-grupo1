using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConferenceApp.Data;
using ConferenceApp.Models;

namespace ConferenceApp.Controllers
{
    public class TalkController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TalkController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Talk
        public async Task<IActionResult> Index()
        {
            return View(await _context.Talks.ToListAsync());
        }

        // GET: Talk/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talk = await _context.Talks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (talk == null)
            {
                return NotFound();
            }

            return View(talk);
        }

        // GET: Talk/Create
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

        // POST: Talk/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Topic,ComplementaryMaterial,Id,Name,StartDate,EndDate,ConferenceVersionId,RoomId")] Talk talk)
        {
            if (ModelState.IsValid)
            {
                _context.Add(talk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(talk);
        }

        // GET: Talk/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talk = await _context.Talks.FindAsync(id);
            if (talk == null)
            {
                return NotFound();
            }
            var conferenceVersion = await _context.ConferenceVersions.Where(x => x.Id == talk.ConferenceVersionId).FirstOrDefaultAsync();
            var rooms = await _context.Rooms.Where(x => x.EventCentreId == conferenceVersion.EventCentreId).ToListAsync();
            this.ViewData["Rooms"] = new SelectList(rooms, "Id", "Name");
            return View(talk);
        }

        // POST: Talk/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Topic,ComplementaryMaterial,Id,Name,StartDate,EndDate,ConferenceVersionId,RoomId")] Talk talk)
        {
            if (id != talk.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(talk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TalkExists(talk.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(talk);
        }

        // GET: Talk/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var talk = await _context.Talks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (talk == null)
            {
                return NotFound();
            }

            return View(talk);
        }

        // POST: Talk/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var talk = await _context.Talks.FindAsync(id);
            _context.Talks.Remove(talk);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TalkExists(int id)
        {
            return _context.Talks.Any(e => e.Id == id);
        }
    }
}
