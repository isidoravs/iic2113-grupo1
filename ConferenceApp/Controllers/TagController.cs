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
    public class TagController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TagController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tag
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tags.ToListAsync());
        }

        // GET: Tag/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            var Chats = await _context.Chats.Where(x => x.EventTags.Any(et => et.TagId == tag.Id)).ToListAsync();
            var Talks = await _context.Talks.Where(x => x.EventTags.Any(et => et.TagId == tag.Id)).ToListAsync();
            var PracticalSessions = await _context.PracticalSessions.Where(x => x.EventTags.Any(et => et.TagId == tag.Id)).ToListAsync();

            var ChatAttendants = await _context.Roles.Where(x => Chats.Any(c => c.Id == x.EventId)).ToListAsync();
            var TalkAttendants = await _context.Roles.Where(x => Talks.Any(t => t.Id == x.EventId)).ToListAsync();
            var PracticalSessionAttendants = await _context.Roles.Where(x => PracticalSessions.Any(ps => ps.Id == x.EventId)).ToListAsync();

            var TotalAttendantsRoles = ChatAttendants.Union(TalkAttendants).Union(PracticalSessionAttendants).Distinct();
            var TotalAttendantsUsersNum = await _context.Users.Where(u => TotalAttendantsRoles.Any(r => r.UserId == u.Id)).CountAsync();

            var FeedbackCategories = await _context.FeedbackCategories.ToListAsync();
            var Feedbacks = await _context.Feedbacks.Where(f => Chats.Any(c => c.Id == f.EventId) || Talks.Any(t => t.Id == f.EventId) || PracticalSessions.Any(ps => ps.Id == f.EventId)).ToListAsync();

            var FeedbackAveragePerCategory = new List<object>();
            var FeedbackCategoryName = new List<object>();

            foreach (var Category in FeedbackCategories)
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








            ViewBag.TotalAttendantsUsersNum = TotalAttendantsUsersNum;
            ViewBag.FeedbackCategoryName = FeedbackCategoryName;
            ViewBag.FeedbackAveragePerCategory = FeedbackAveragePerCategory;

            return View(tag);
        }

        // GET: Tag/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tag/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Tag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.Id))
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
            return View(tag);
        }

        // GET: Tag/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }
    }
}
