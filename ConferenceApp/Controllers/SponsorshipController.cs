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
    public class SponsorshipController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SponsorshipController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sponsorship
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sponsorships.ToListAsync());
        }

        // GET: Sponsorship/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsorship = await _context.Sponsorships
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sponsorship == null)
            {
                return NotFound();
            }

            return View(sponsorship);
        }

        // GET: Sponsorship/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sponsorship/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ConferenceVersionId,SponsorId")] Sponsorship sponsorship)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sponsorship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sponsorship);
        }

        // GET: Sponsorship/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsorship = await _context.Sponsorships.FindAsync(id);
            if (sponsorship == null)
            {
                return NotFound();
            }
            return View(sponsorship);
        }

        // POST: Sponsorship/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ConferenceVersionId,SponsorId")] Sponsorship sponsorship)
        {
            if (id != sponsorship.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sponsorship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponsorshipExists(sponsorship.Id))
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
            return View(sponsorship);
        }

        // GET: Sponsorship/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsorship = await _context.Sponsorships
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sponsorship == null)
            {
                return NotFound();
            }

            return View(sponsorship);
        }

        // POST: Sponsorship/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sponsorship = await _context.Sponsorships.FindAsync(id);
            _context.Sponsorships.Remove(sponsorship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SponsorshipExists(int id)
        {
            return _context.Sponsorships.Any(e => e.Id == id);
        }
    }
}
