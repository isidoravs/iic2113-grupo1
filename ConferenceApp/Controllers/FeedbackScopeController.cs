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
    public class FeedbackScopeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackScopeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FeedbackScope
        public async Task<IActionResult> Index()
        {
            return View(await _context.FeedbackScopes.ToListAsync());
        }

        // GET: FeedbackScope/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedbackScope = await _context.FeedbackScopes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedbackScope == null)
            {
                return NotFound();
            }

            return View(feedbackScope);
        }

        // GET: FeedbackScope/Create
        public async Task<IActionResult> Create(int? feedbackId)
        {
            ViewBag.FeedbackId = feedbackId;
            var feedback = await _context.Feedbacks.FindAsync(feedbackId);
            ViewBag.Event = await _context.Events.FindAsync(feedback.EventId);
            var categories = await _context.FeedbackCategories.ToListAsync();
            ViewData["Categories"] = new SelectList(categories,"Id","Name");
            
            return View();
        }

        // POST: FeedbackScope/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Grade,FeedbackId,FeedbackCategoryId")] FeedbackScope feedbackScope, string send, string nextCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedbackScope);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(send))
                {
                    return RedirectToAction("Details", "Feedback", new {id = feedbackScope.FeedbackId});
                }
                else if (!string.IsNullOrEmpty(nextCategory))
                {
                    return RedirectToAction("Create", "FeedbackScope", new {feedbackId = feedbackScope.FeedbackId});
                }
                return RedirectToAction(nameof(Index));
            }
            return View(feedbackScope);
        }

        // GET: FeedbackScope/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedbackScope = await _context.FeedbackScopes.FindAsync(id);
            if (feedbackScope == null)
            {
                return NotFound();
            }
            return View(feedbackScope);
        }

        // POST: FeedbackScope/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Grade,FeedbackId,FeedbackCategoryId")] FeedbackScope feedbackScope)
        {
            if (id != feedbackScope.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedbackScope);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackScopeExists(feedbackScope.Id))
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
            return View(feedbackScope);
        }

        // GET: FeedbackScope/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedbackScope = await _context.FeedbackScopes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedbackScope == null)
            {
                return NotFound();
            }

            return View(feedbackScope);
        }

        // POST: FeedbackScope/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedbackScope = await _context.FeedbackScopes.FindAsync(id);
            _context.FeedbackScopes.Remove(feedbackScope);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackScopeExists(int id)
        {
            return _context.FeedbackScopes.Any(e => e.Id == id);
        }
    }
}
