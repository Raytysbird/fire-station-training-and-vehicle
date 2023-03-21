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
    public class AssignedTaskController : Controller
    {
        private readonly FireFighterContext _context;

        public AssignedTaskController(FireFighterContext context)
        {
            _context = context;
        }

        // GET: AssignedTask
        public async Task<IActionResult> Index()
        {
              return _context.AssignedTasks != null ? 
                          View(await _context.AssignedTasks.ToListAsync()) :
                          Problem("Entity set 'FireFighterContext.AssignedTasks'  is null.");
        }

        // GET: AssignedTask/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AssignedTasks == null)
            {
                return NotFound();
            }

            var assignedTask = await _context.AssignedTasks
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (assignedTask == null)
            {
                return NotFound();
            }

            return View(assignedTask);
        }

        // GET: AssignedTask/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AssignedTask/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,UserId")] AssignedTask assignedTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignedTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(assignedTask);
        }

        // GET: AssignedTask/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AssignedTasks == null)
            {
                return NotFound();
            }

            var assignedTask = await _context.AssignedTasks.FindAsync(id);
            if (assignedTask == null)
            {
                return NotFound();
            }
            return View(assignedTask);
        }

        // POST: AssignedTask/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,UserId")] AssignedTask assignedTask)
        {
            if (id != assignedTask.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignedTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignedTaskExists(assignedTask.TaskId))
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
            return View(assignedTask);
        }

       

        // POST: AssignedTask/Delete/5
     
        public async Task<IActionResult> Delete(int id,string userId)
        {
            if (_context.AssignedTasks == null)
            {
                return Problem("Entity set 'FireFighterContext.AssignedTasks'  is null.");
            }
            var assignedTask = await _context.AssignedTasks.Where(x=>x.TaskId==id).Where(x=>x.UserId==userId).FirstOrDefaultAsync();
            if (assignedTask != null)
            {
                _context.AssignedTasks.Remove(assignedTask);
            }
            
            await _context.SaveChangesAsync();
            TempData["Success"] = "User unassigned successfully!!";
            return RedirectToAction("Details","UserTask", new { id = id });
        }

        private bool AssignedTaskExists(int id)
        {
          return (_context.AssignedTasks?.Any(e => e.TaskId == id)).GetValueOrDefault();
        }
    }
}
