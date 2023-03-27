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
            var userId = _userManager.GetUserId(HttpContext.User);

            var currentTasks = await _context.AssignedTasks.Include(x => x.Task).Include(x => x.Task.Course).Where(x => x.UserId == userId).Where(x => x.Task.LastDate >= DateTime.Now && x.IsComplete==null).ToListAsync();
            ViewBag.CurrentTasks = currentTasks;

            var completeTasks = await _context.AssignedTasks.Include(x => x.Task).Include(x => x.Task.Course).Where(x => x.UserId == userId).Where(x => x.IsComplete == true).ToListAsync();
            ViewBag.CompleteTasks = completeTasks;

            var dueTasks = await _context.AssignedTasks.Include(x => x.Task).Include(x => x.Task.Course).Where(x => x.UserId == userId).Where(x => x.Task.LastDate <= DateTime.Now && x.IsComplete == null).ToListAsync();
            ViewBag.DueTasks = dueTasks;

            return View();
        }

        // GET: UserTask/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserTasks == null)
            {
                return NotFound();
            }

            var assignedUsers = _context.AssignedTasks.Include(x => x.User).Where(x => x.TaskId == id).ToList();

            var notAssignedUsers = _context.AspNetUsers
                                     .Where(u => !_context.AssignedTasks
                                     .Any(ut => ut.UserId == u.Id && ut.TaskId == id))
                                     .ToList();

            if (assignedUsers != null)
            {
                ViewBag.Assigned = assignedUsers;

            }
            if (notAssignedUsers != null)
            {
                ViewBag.UnAssigned = notAssignedUsers;


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
        public async Task<IActionResult> CompleteTask(int? id)
        {
            if (id == null || _context.UserTasks == null)
            {
                return NotFound();
            }

            var userTask = await _context.UserTasks
               .Include(u => u.Course)
               .Include(x=>x.AssignedTasks)
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
            //ViewBag.Users=_context.AspNetUsers.ToList();
            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Name");
            var tasks = _context.UserTasks.Include(x => x.Course).Where(x => x.LastDate > DateTime.Now).ToList();
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
                TempData["Success"] = "New Task Created successfully!!";
                return RedirectToAction("Create");
            }
            //ViewBag.Users = _context.AspNetUsers.Where.ToList();

            ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "Name", userTask.CourseId);
            return RedirectToAction("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaskConsent(UserTask userTask, bool consent)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var task = await _context.AssignedTasks.Where(x => x.TaskId == userTask.TaskId).Where(x => x.UserId == userId).FirstOrDefaultAsync();
            task.IsComplete = true;
            if (task != null)
            {
                _context.Update(task);
                TempData["Success"] = "Task completed successfully!!";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AssignUser(List<string> selectedValues, int id)
        {
            AssignedTask assigned = new AssignedTask();
            foreach (var item in selectedValues)
            {
                assigned.TaskId = id;
                assigned.UserId = item;
                _context.Add(assigned);
                await _context.SaveChangesAsync();
                TempData["Success"] = "User assigned successfully!!";
                TempData["RedirectUrl"] = Url.Action("Create", "UserTask");
            }
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
