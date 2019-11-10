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
    public class FoodServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FoodService
        public async Task<IActionResult> Index()
        {
            return View(await _context.FoodServices.ToListAsync());
        }

        // GET: FoodService/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodService = await _context.FoodServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodService == null)
            {
                return NotFound();
            }

            return View(foodService);
        }

        // GET: FoodService/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodService/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Category,Id,Name,StartDate,EndDate,ConferenceVersionId")] FoodService foodService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(foodService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(foodService);
        }

        // GET: FoodService/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodService = await _context.FoodServices.FindAsync(id);
            if (foodService == null)
            {
                return NotFound();
            }
            return View(foodService);
        }

        // POST: FoodService/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Category,Id,Name,StartDate,EndDate,ConferenceVersionId")] FoodService foodService)
        {
            if (id != foodService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(foodService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodServiceExists(foodService.Id))
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
            return View(foodService);
        }

        // GET: FoodService/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodService = await _context.FoodServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodService == null)
            {
                return NotFound();
            }

            return View(foodService);
        }

        // POST: FoodService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodService = await _context.FoodServices.FindAsync(id);
            _context.FoodServices.Remove(foodService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodServiceExists(int id)
        {
            return _context.FoodServices.Any(e => e.Id == id);
        }
    }
}
