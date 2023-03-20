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
    public class UserTaskController : Controller
    {
        private readonly FireFighterContext _context;
        private readonly UserManager<User> _userManager;

        public UserTaskController(FireFighterContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserTask
        public async Task<IActionResult> Index()
        {
            var fireFighterContext = _context.UserTasks.Include(u => u.Course);
            return View(await fireFighterContext.ToListAsync());
        }

        // GET: UserTask/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserTasks == null)
            {
                return NotFound();
            }
            ViewBag.Users = _context.AspNetUsers.ToList();
            var userTask = await _context.UserTasks
                .Include(u => u.Course)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (userTask == null)
            {
                return NotFound();
            }

            return View(userTask);
        }

        // GET: UserTask/Create
        public IActionResult Create()
        {
            ViewBag.Users=_context.AspNetUsers.ToList();
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Name");
            var tasks=_context.UserTasks.Include(x=>x.Course).Where(x=>x.LastDate>DateTime.Now).ToList();
            ViewBag.Tasks = tasks;
            return View();
        }

        // POST: UserTask/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,CourseId,LastDate,IsCompleted")] UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = _context.AspNetUsers.ToList();
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Name", userTask.CourseId);
            return View(userTask);
        }
        [HttpPost]
        public IActionResult AssignUser(List<string> selectedValues, int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            foreach (var item in selectedValues)
            {
               
            }
            // process selected values
            return Ok();
        }

        // GET: UserTask/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserTasks == null)
            {
                return NotFound();
            }

            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", userTask.CourseId);
            return View(userTask);
        }

        // POST: UserTask/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,CourseId,LastDate,IsCompleted")] UserTask userTask)
        {
            if (id != userTask.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTaskExists(userTask.TaskId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId", userTask.CourseId);
            return View(userTask);
        }

        // GET: UserTask/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserTasks == null)
            {
                return NotFound();
            }

            var userTask = await _context.UserTasks
                .Include(u => u.Course)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (userTask == null)
            {
                return NotFound();
            }

            return View(userTask);
        }

        // POST: UserTask/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserTasks == null)
            {
                return Problem("Entity set 'FireFighterContext.UserTasks'  is null.");
            }
            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask != null)
            {
                _context.UserTasks.Remove(userTask);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTaskExists(int id)
        {
          return (_context.UserTasks?.Any(e => e.TaskId == id)).GetValueOrDefault();
        }
    }
}
