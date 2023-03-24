using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fire_station_training_and_vehicle.Models;

namespace fire_station_training_and_vehicle.Controllers
{
    public class AssignedEventController : Controller
    {
        private readonly FireFighterContext _context;

        public AssignedEventController(FireFighterContext context)
        {
            _context = context;
        }

        // GET: AssignedEvent
        public async Task<IActionResult> Index()
        {
            var fireFighterContext = _context.AssignedEvents.Include(a => a.Event).Include(a => a.User);
            return View(await fireFighterContext.ToListAsync());
        }

        // GET: AssignedEvent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AssignedEvents == null)
            {
                return NotFound();
            }

            var assignedEvent = await _context.AssignedEvents
                .Include(a => a.Event)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (assignedEvent == null)
            {
                return NotFound();
            }

            return View(assignedEvent);
        }

        // GET: AssignedEvent/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: AssignedEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,UserId,Attended,Grade")] AssignedEvent assignedEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignedEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", assignedEvent.EventId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", assignedEvent.UserId);
            return View(assignedEvent);
        }

        // GET: AssignedEvent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AssignedEvents == null)
            {
                return NotFound();
            }

            var assignedEvent = await _context.AssignedEvents.FindAsync(id);
            if (assignedEvent == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", assignedEvent.EventId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", assignedEvent.UserId);
            return View(assignedEvent);
        }

        // POST: AssignedEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,UserId,Attended,Grade")] AssignedEvent assignedEvent)
        {
            if (id != assignedEvent.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignedEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignedEventExists(assignedEvent.EventId))
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
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", assignedEvent.EventId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", assignedEvent.UserId);
            return View(assignedEvent);
        }

        public async Task<IActionResult> Delete(int id, string userId)
        {
            if (_context.AssignedTasks == null)
            {
                return Problem("Entity set 'FireFighterContext.AssignedTasks'  is null.");
            }
            var assignedEvent = await _context.AssignedEvents.Where(x => x.EventId == id).Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (assignedEvent != null)
            {
                _context.AssignedEvents.Remove(assignedEvent);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "User unassigned successfully!!";
            return RedirectToAction("Details", "Event", new { id = id });
        }

        private bool AssignedEventExists(int id)
        {
          return (_context.AssignedEvents?.Any(e => e.EventId == id)).GetValueOrDefault();
        }
    }
}
