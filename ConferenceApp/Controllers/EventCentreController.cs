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
    public class EventCentreController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventCentreController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EventCentre
        public async Task<IActionResult> Index()
        {
            return View(await _context.EventCentres.ToListAsync());
        }

        // GET: EventCentre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCentre = await _context.EventCentres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCentre == null)
            {
                return NotFound();
            }

            var eventCentresRooms = await _context.Rooms.Where(x => x.EventCentreId == eventCentre.Id).ToListAsync();
            ViewBag.eventCentresRooms = eventCentresRooms;

            return View(eventCentre);
        }

        // GET: EventCentre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EventCentre/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,MapImage,Latitude,Longitude")] EventCentre eventCentre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventCentre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventCentre);
        }

        // GET: EventCentre/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCentre = await _context.EventCentres.FindAsync(id);
            if (eventCentre == null)
            {
                return NotFound();
            }
            return View(eventCentre);
        }

        // POST: EventCentre/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,MapImage,Latitude,Longitude")] EventCentre eventCentre)
        {
            if (id != eventCentre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventCentre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventCentreExists(eventCentre.Id))
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
            return View(eventCentre);
        }

        // GET: EventCentre/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventCentre = await _context.EventCentres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCentre == null)
            {
                return NotFound();
            }

            return View(eventCentre);
        }

        // POST: EventCentre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventCentre = await _context.EventCentres.FindAsync(id);
            _context.EventCentres.Remove(eventCentre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventCentreExists(int id)
        {
            return _context.EventCentres.Any(e => e.Id == id);
        }
    }
}
