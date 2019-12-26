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
            var userNotifications = await _context.Notifications.Where(
                notification => notification.ReceiverId == userId).ToListAsync();
            
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
        public IActionResult Create(int eventId, int conferenceVersionId, string eventName, string conferenceName, int conferenceVersionNumber, string eventType)
        {
            var conferenceVersionFullName = $"Conferencia {conferenceName} versión {conferenceVersionNumber.ToString()}";
            var eventFullName = $"Evento {eventName}";
            var eventOrConference = new List<string>() {eventFullName, conferenceVersionFullName};
            
            var senderUserEmail = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var receiverOptions = new List<string>() {"Asistentes", "Expositores", "Asistentes y Expositores"};

            ViewBag.EventOrConference = new SelectList(eventOrConference);
            ViewBag.SenderUserEmail = senderUserEmail;
            ViewBag.ReceiverOptions = new SelectList(receiverOptions);
            ViewBag.EventId = eventId;
            ViewBag.ConferenceVersionId = conferenceVersionId;
            ViewBag.EventType = eventType;
            return View();
        }

        // POST: Notification/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string message, string subject, string senderUserEmail,
            string receivers, string eventOrConference, int eventId, int conferenceVersionId, string eventType)
        {
            // notificacion para todos asistentes y/o expositores de un EVENTO
            if (eventOrConference.IndexOf("Evento") >= 0)
            {
                if (receivers == "Asistentes")
                {
                    await SendNotificationToEventAttendants(eventId, subject, message, senderUserEmail, true);
                }
                else if (receivers == "Expositores")
                {
                    await SendNotificationToEventExhibitors(eventId, subject, message, senderUserEmail, true,
                        eventType);
                }
                else
                {
                    await SendNotificationToEventAttendants(eventId, subject, message, senderUserEmail, true);
                    await SendNotificationToEventExhibitors(eventId, subject, message, senderUserEmail, true,
                        eventType);
                }

            }
            // notificacion para todos asistentes y/o expositores de la VERSION DE CONFERENCIA
            else
            {
                if (receivers == "Asistentes")
                {
                    await SendNotificationToConferenceVersionAttendants(conferenceVersionId, subject, message,
                        senderUserEmail);
                }
                else if (receivers == "Expositores")
                {
                    await SendNotificationToConferenceVersionExhibitors(conferenceVersionId, subject, message,
                        senderUserEmail, eventType);
                }
                else
                {
                    await SendNotificationToConferenceVersionAttendants(conferenceVersionId, subject, message,
                        senderUserEmail);
                    await SendNotificationToConferenceVersionExhibitors(conferenceVersionId, subject, message,
                        senderUserEmail, eventType);
                }
            }

            return RedirectToAction("Details", "Event", new {id = eventId});
        }

        public async Task SendNotificationToEventAttendants(int eventId, string subject, string message, string senderUserEmail, bool isEventNotification)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            var conferenceVersion =
                await _context.ConferenceVersions.FirstOrDefaultAsync(x => x.Id == @event.ConferenceVersionId);
            var conference = await _context.Conferences.FirstOrDefaultAsync(x => x.Id == conferenceVersion.ConferenceId);
            var conferenceVersionFullName = $"{conference.Name} versión {conferenceVersion.Number.ToString()}";
            
            var attendants = await _context.Roles.Where(role => role.EventId == eventId && role.Name == "attendant").ToListAsync();
            foreach (var attendant in attendants)
            {
                var notification = new Notification(subject, message, attendant.UserId, senderUserEmail, isEventNotification);
                notification.EventId = @event.Id;
                notification.EventName = @event.Name;
                notification.ConferenceId = conferenceVersion.Id;
                notification.ConferenceName = conferenceVersionFullName;
                _context.Add(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendNotificationToConferenceVersionAttendants(int conferenceVersionId, string subject, string message, string senderUserEmail)
        {
            var converenceVersionEvents =
                await _context.Events.Where(x => x.ConferenceVersionId == conferenceVersionId).ToListAsync();
            foreach (var @event in converenceVersionEvents)
            {
                await SendNotificationToEventAttendants(@event.Id, subject, message, senderUserEmail, false);
            }
        }

        public async Task SendNotificationToConferenceVersionExhibitors(int conferenceVersionId, string subject, string message,
            string senderUserEmail, string eventType)
        {
            var converenceVersionEvents =
                await _context.Events.Where(x => x.ConferenceVersionId == conferenceVersionId).ToListAsync();
            foreach (var @event in converenceVersionEvents)
            {
                await SendNotificationToEventExhibitors(@event.Id, subject, message, senderUserEmail, false, eventType);
            }
        }

        public async Task SendNotificationToEventExhibitors(int eventId, string subject, string message,
            string senderUserEmail, bool isEventNotification, string eventType)
        {
            if (eventType == "Chat")
            {
                var chat = await _context.Chats.FirstOrDefaultAsync(e => e.Id == eventId);
                var conferenceVersion =
                    await _context.ConferenceVersions.FirstOrDefaultAsync(x => x.Id == chat.ConferenceVersionId);
                var conference = await _context.Conferences.FirstOrDefaultAsync(x => x.Id == conferenceVersion.ConferenceId);
                var conferenceVersionFullName = $"{conference.Name} versión {conferenceVersion.Number.ToString()}";
                
                var notification = new Notification(subject, message, chat.Moderator, senderUserEmail, isEventNotification);
                notification.EventId = chat.Id;
                notification.EventName = chat.Name;
                notification.ConferenceId = conferenceVersion.Id;
                notification.ConferenceName = conferenceVersionFullName;
                _context.Add(notification);
                await _context.SaveChangesAsync();
            }
            else if (eventType == "Practical Session")
            {
                var practicalSession = await _context.PracticalSessions.FirstOrDefaultAsync(e => e.Id == eventId);
                var conferenceVersion =
                    await _context.ConferenceVersions.FirstOrDefaultAsync(x => x.Id == practicalSession.ConferenceVersionId);
                var conference = await _context.Conferences.FirstOrDefaultAsync(x => x.Id == conferenceVersion.ConferenceId);
                var conferenceVersionFullName = $"{conference.Name} versión {conferenceVersion.Number.ToString()}";
                
                var notification = new Notification(subject, message, practicalSession.Exhibitor, senderUserEmail, isEventNotification);
                notification.EventId = practicalSession.Id;
                notification.EventName = practicalSession.Name;
                notification.ConferenceId = conferenceVersion.Id;
                notification.ConferenceName = conferenceVersionFullName;
                _context.Add(notification);
                await _context.SaveChangesAsync();
            }
            else if (eventType == "Talk")
            {
                var talk = await _context.Talks.FirstOrDefaultAsync(e => e.Id == eventId);
                var conferenceVersion =
                    await _context.ConferenceVersions.FirstOrDefaultAsync(x => x.Id == talk.ConferenceVersionId);
                var conference = await _context.Conferences.FirstOrDefaultAsync(x => x.Id == conferenceVersion.ConferenceId);
                var conferenceVersionFullName = $"{conference.Name} versión {conferenceVersion.Number.ToString()}";
                
                var notification = new Notification(subject, message, talk.Exhibitor, senderUserEmail, isEventNotification);
                notification.EventId = talk.Id;
                notification.EventName = talk.Name;
                notification.ConferenceId = conferenceVersion.Id;
                notification.ConferenceName = conferenceVersionFullName;
                _context.Add(notification);
                await _context.SaveChangesAsync();
            }
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