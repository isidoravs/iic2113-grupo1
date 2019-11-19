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
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAssistant = await _context.Roles.Where(x => (x.UserId == currentUserId && x.EventId == foodService.Id)).ToListAsync();
            
            int assisting = isAssistant.Count;
            ViewBag.assisting = assisting;


            return View(foodService);
        }

        // GET: FoodService/Create
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
            
            this.ViewData["CategoryOptions"] = new List<SelectListItem>
            {
                new SelectListItem {Text = "Almuerzo", Value = "lunch"},
                new SelectListItem {Text = "Cena", Value = "dinner"},
                new SelectListItem {Text = "Otro", Value = "other"}
            };
            this.ViewData["Rooms"] = new SelectList(rooms, "Id", "Name");
            return View();
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
        // POST: FoodService/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Category,Id,Name,StartDate,EndDate,ConferenceVersionId, RoomId")] FoodService foodService)
        {
            var conferenceVersion = await _context.ConferenceVersions.Where(x => x.Id == foodService.ConferenceVersionId).FirstOrDefaultAsync();
            if (conferenceVersion.StartDate > foodService.StartDate || conferenceVersion.EndDate < foodService.EndDate)
            {
                // hay problemas con la fecha
                TempData["DateError"] = "Valor temporal";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Add(foodService);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = foodService.Id.ToString() });
                }
            }
            return RedirectToAction("Index", "Event");
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
            
            this.ViewData["CategoryOptions"] = new List<SelectListItem>
            {
                new SelectListItem {Text = "Almuerzo", Value = "lunch"},
                new SelectListItem {Text = "Cena", Value = "dinner"},
                new SelectListItem {Text = "Otro", Value = "other"}
            };
            var conferenceVersion = await _context.ConferenceVersions.Where(x => x.Id == foodService.ConferenceVersionId).FirstOrDefaultAsync();
            var rooms = await _context.Rooms.Where(x => x.EventCentreId == conferenceVersion.EventCentreId).ToListAsync();
            this.ViewData["Rooms"] = new SelectList(rooms, "Id", "Name");

            return View(foodService);
        }

        // POST: FoodService/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Category,Id,Name,StartDate,EndDate,ConferenceVersionId, RoomId")] FoodService foodService)
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
