using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConferenceApp.Data;
using Microsoft.AspNetCore.Mvc;
using ConferenceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConferenceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var assistingToEvents = await _context.Roles.Where(x => (x.UserId == currentUserId && x.Name == "attendant")).ToListAsync();
            var eventsToList = new List<Event>() {};
            foreach (var role in assistingToEvents)
            {
                var @event = await _context.Events.FirstOrDefaultAsync(m => m.Id == role.EventId);
                eventsToList.Add(@event);
            }

            var admin = false;
            var adminList = await _context.Admins.Where(x => x.UserId == currentUserId).ToListAsync();
            if (adminList.Count > 0)
            {
                admin = true;
            }
            ViewBag.admin = admin;
            
            ViewBag.eventsToList = eventsToList;
            
            
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Admin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}