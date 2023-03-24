using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fire_station_training_and_vehicle.Models;
using Microsoft.AspNetCore.Identity;

namespace fire_station_training_and_vehicle.Controllers
{
    public class EventController : Controller
    {
        private readonly FireFighterContext _context;
        private readonly UserManager<User> _userManager;

        public EventController(FireFighterContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            //var userId = _userManager.GetUserId(HttpContext.User);

            //var currentTasks = await _context.AssignedEvents.Include(x => x.Event).Include(x => x.Event.Course).Where(x => x.UserId == userId).Where(x => x.Event.LastDate >= DateTime.Now && x.IsComplete == null).ToListAsync();
            //ViewBag.CurrentTasks = currentTasks;

            //var completeTasks = await _context.AssignedEvents.Include(x => x.Event).Include(x => x.Event.Course).Where(x => x.UserId == userId).Where(x => x.IsComplete == true).ToListAsync();
            //ViewBag.CompleteTasks = completeTasks;

            //var dueTasks = await _context.AssignedEvents.Include(x => x.Event).Include(x => x.Event.Course).Where(x => x.UserId == userId).Where(x => x.Event.LastDate <= DateTime.Now && x.IsComplete == null).ToListAsync();
            //ViewBag.DueTasks = dueTasks;

            return View();
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Name", @event.CourseId);
            ViewData["TeacherId"] = new SelectList(_context.AspNetUsers, "Id", "FirstName");
            return View(@event);
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
                .Include(x=>x.Course)
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
