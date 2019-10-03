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
    public class FeedbackCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FeedbackCategory
        public async Task<IActionResult> Index()
        {
            return View(await _context.FeedbackCategories.ToListAsync());
        }

        // GET: FeedbackCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedbackCategory = await _context.FeedbackCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedbackCategory == null)
            {
                return NotFound();
            }

            return View(feedbackCategory);
        }

        // GET: FeedbackCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FeedbackCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] FeedbackCategory feedbackCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedbackCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedbackCategory);
        }

        // GET: FeedbackCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedbackCategory = await _context.FeedbackCategories.FindAsync(id);
            if (feedbackCategory == null)
            {
                return NotFound();
            }
            return View(feedbackCategory);
        }

        // POST: FeedbackCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] FeedbackCategory feedbackCategory)
        {
            if (id != feedbackCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedbackCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackCategoryExists(feedbackCategory.Id))
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
            return View(feedbackCategory);
        }

        // GET: FeedbackCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedbackCategory = await _context.FeedbackCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedbackCategory == null)
            {
                return NotFound();
            }

            return View(feedbackCategory);
        }

        // POST: FeedbackCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedbackCategory = await _context.FeedbackCategories.FindAsync(id);
            _context.FeedbackCategories.Remove(feedbackCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackCategoryExists(int id)
        {
            return _context.FeedbackCategories.Any(e => e.Id == id);
        }
    }
}
