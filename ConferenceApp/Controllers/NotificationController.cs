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
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notification
        public async Task<IActionResult> Index()
        {
            return View(await _context.Notifications.ToListAsync());
        }
        
        // GET: Notification/UserNotifications

        public async Task<IActionResult> UserNotifications(string userId)
        {
            Console.WriteLine($"USER ID: {userId}");
            var userNotifications = await _context.Notifications.Where(
                notification => notification.ReceiverId == userId).ToListAsync();

            foreach (var notification in userNotifications)
            {
                Console.WriteLine($"Notification.Event.Name = {notification.EventName}");
            }
            ViewBag.UserNotifications = userNotifications;

            return View();
        }

        // GET: Notification/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Notification/Create
        public IActionResult Create(int eventId, int conferenceId)
        {
            var senderId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var receiverOptions = new List<string>() {"Asistentes", "Expositores", "Asistentes y Expositores"};

            Console.WriteLine($"AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA {eventId}");

            ViewBag.SenderId = senderId;
            ViewBag.ReceiverOptions = new SelectList(receiverOptions);
            ViewBag.EventId = eventId;
            ViewBag.ConferenceId = conferenceId;
            return View();
        }

        // POST: Notification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string message, string subject, string senderId, string receivers, int eventId, int conferenceId)
        {
            // notificacion para todos asistentes y/o expositores de un EVENTO
            if (eventId != -1)
            {
                var practicalSession = await _context.PracticalSessions.FirstOrDefaultAsync(e => e.Id == eventId);
                if (receivers.IndexOf("Asistentes") >= 0)
                {
                    var attendants = await _context.Roles.Where(role => role.EventId == eventId && role.Name == "attendant").ToListAsync();
                    foreach (var attendant in attendants)
                    {
                        var notification = new Notification(subject, message, senderId, attendant.UserId);
                        notification.EventId = practicalSession.Id;
                        notification.EventName = practicalSession.Name;
                        notification.IsEventNotification = true;
                        _context.Add(notification);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
                //return View(practicalSession);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            //return RedirectToAction(nameof(Index));
        }

        // GET: Notification/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Notification/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Message")] Notification notification)
        {
            if (id != notification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.Id))
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
            return View(notification);
        }

        // GET: Notification/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Notification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationExists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}