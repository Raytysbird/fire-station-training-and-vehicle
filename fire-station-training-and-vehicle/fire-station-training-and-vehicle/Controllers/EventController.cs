using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fire_station_training_and_vehicle.Models;
using Microsoft.AspNetCore.Identity;
using fire_station_training_and_vehicle.Models.Metadata;
using ClosedXML.Excel;
using System.Security.Claims;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace fire_station_training_and_vehicle.Controllers
{
    public class EventController : Controller
    {
        private readonly FireFighterContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public EventController(FireFighterContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> TeacherEvents()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var events = _context.Events.Include(x=>x.Course).Where( x => x.TeacherId == userId && x.StartDate <= DateTime.Now && x.EndDate >=DateTime.Now).ToList();


            return View(events);
        }
        public async Task<IActionResult> MarkAttendence(int? id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var assignedEventUsers = _context.AssignedEvents.Include(x=>x.Event).Include(x => x.User).Where(x => x.EventId == id && x.Attended==null).ToList();
            ViewBag.AttendUser = assignedEventUsers;
            return View();
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> PostAttendence(List<string> selectedValues, int id)
        {
          
            AssignedEvent assigned = new AssignedEvent();
            foreach (var item in selectedValues)
            {
                var events=_context.AssignedEvents.Where(x=>x.UserId.Contains(item) && x.EventId==id).ToList();
                foreach (var e in events)
                {
                    e.Attended = true;
                    _context.Update(e);
                    await _context.SaveChangesAsync();
                }
                TempData["Success"] = "Attendence successfully!!";
                TempData["RedirectUrl"] = Url.Action("Create", "UserTask");
            }
            return Ok();
        }
        public async Task<IActionResult> Events()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            var scheduledEvents = await _context.AssignedEvents.Include(x => x.Event).Include(x => x.Event.Course).Where(x => x.UserId == userId).Where(x => x.Event.StartDate >= DateTime.Now && x.Attended == null).ToListAsync();
            ViewBag.ScheduledEvents = scheduledEvents;

            var completeEvents = await _context.AssignedEvents.Include(x => x.Event).Include(x => x.Event.Course).Where(x => x.UserId == userId).Where(x => x.Attended == true).ToListAsync();
            ViewBag.CompleteEvents = completeEvents;

            return View();
        }
        public IEnumerable<CalendarEvent> AllEvents()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            CalendarEvent e = new CalendarEvent();
            List<CalendarEvent> events = new List<CalendarEvent>();
            // var a = _context.Events.Include(x=>x.Course).Include(x=>x.AssignedEvents).Where(x=>x.AssignedEvents.).ToList();
            var a = _context.AssignedEvents.Include(x => x.Event).Include(x => x.Event.Course).Where(x => x.UserId == userId).ToList();
            foreach (var item in a)
            {
                e.Id = item.EventId;
                e.Title = item.Event.Course.Name;
                e.Start = (DateTime)item.Event.StartDate;
                e.End = (DateTime)item.Event.EndDate;
                e.Location = item.Event.Location;
                events.Add(e);
            }
            return events;
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var assignedEventUsers = _context.AssignedEvents.Include(x => x.User).Where(x => x.EventId == id).ToList();

            var notAssignedEventUsers = _context.AspNetUsers
                                     .Where(u => !_context.AssignedEvents
                                     .Any(ut => ut.UserId == u.Id && ut.EventId == id))
                                     .ToList();

            if (assignedEventUsers != null)
            {
                ViewBag.AssignedEvent = assignedEventUsers;

            }
            if (notAssignedEventUsers != null)
            {
                ViewBag.UnAssignedEvent = notAssignedEventUsers;


            }
            var userEvent = await _context.Events
                .Include(u => u.Course)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (userEvent == null)
            {
                return NotFound();
            }

            return View(userEvent);
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AssignUser(List<string> selectedValues, int id)
        {
            AssignedEvent assigned = new AssignedEvent();
            foreach (var item in selectedValues)
            {
                assigned.EventId = id;
                assigned.UserId = item;
                _context.Add(assigned);
                await _context.SaveChangesAsync();
                TempData["Success"] = "User assigned successfully!!";
                TempData["RedirectUrl"] = Url.Action("Create", "UserTask");
            }
            return Ok();
        }
        // GET: Event/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Name");
            ViewData["TeacherId"] = new SelectList(_context.AspNetUsers, "Id", "FirstName");
            var events = _context.Events.Include(x => x.Course).Where(x => x.EndDate > DateTime.Now).ToList();
            ViewBag.Events = events;
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,StartDate,EndDate,Location,TeacherId,CourseId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create");
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Name", @event.CourseId);
            ViewData["TeacherId"] = new SelectList(_context.AspNetUsers, "Id", "FirstName");
            return RedirectToAction("Create");
        }

        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", @event.CourseId);
            return View(@event);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,StartDate,EndDate,Location,TeacherId,CourseId")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", @event.CourseId);
            return View(@event);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(x => x.Course)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'FireFighterContext.Events'  is null.");
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return (_context.Events?.Any(e => e.EventId == id)).GetValueOrDefault();
        }
    }
}
