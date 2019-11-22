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
    public class MenuOptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuOptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MenuOption
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuOptions.ToListAsync());
        }

        // GET: MenuOption/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuOption = await _context.MenuOptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuOption == null)
            {
                return NotFound();
            }

            return View(menuOption);
        }

        // GET: MenuOption/Create
        public async Task<IActionResult> Create(int? foodServiceId)
        {
            var foodServices = foodServiceId == null
                ? await _context.FoodServices.ToListAsync()
                : await _context.FoodServices.Where(x => x.Id == foodServiceId).ToListAsync();
            ViewData["FoodServices"] = new SelectList(foodServices,"Id","Name");
            return View();
        }

        // POST: MenuOption/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,FoodServiceId")] MenuOption menuOption)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuOption);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "FoodService", new { id = menuOption.FoodServiceId.ToString() });
            }
            return View(menuOption);
        }

        // GET: MenuOption/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuOption = await _context.MenuOptions.FindAsync(id);
            if (menuOption == null)
            {
                return NotFound();
            }
            return View(menuOption);
        }

        // POST: MenuOption/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FoodServiceId")] MenuOption menuOption)
        {
            if (id != menuOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuOptionExists(menuOption.Id))
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
            return View(menuOption);
        }

        // GET: MenuOption/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuOption = await _context.MenuOptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuOption == null)
            {
                return NotFound();
            }

            return View(menuOption);
        }

        // POST: MenuOption/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuOption = await _context.MenuOptions.FindAsync(id);
            _context.MenuOptions.Remove(menuOption);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "FoodService", new { id = menuOption.FoodServiceId.ToString() });
        }

        private bool MenuOptionExists(int id)
        {
            return _context.MenuOptions.Any(e => e.Id == id);
        }
    }
}
