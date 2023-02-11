using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fire_station_training_and_vehicle.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using fire_station_training_and_vehicle.Services;


namespace fire_station_training_and_vehicle.Controllers
{

    public class UserController : Controller
    {
        private readonly FireFighterContext _context;
        private readonly UserManager<User> _userManager;

        public UserController(FireFighterContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin,Chief")]
        // GET: User
        public async Task<IActionResult> Index(int pg = 1)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            int pageSize = 9;
            if (pg < 1)
            {
                pg = 1;
            }
            int rescCount = _context.AspNetUsers.Where(x => x.IsDeleted == false).Count();
            var pager = new Pagination(rescCount, pg, pageSize);
            int rescSkip = (pg - 1) * pageSize;
            this.ViewBag.Pager = pager;
            if (User.IsInRole("Admin"))
            {
                var fireFighters = await _context.AspNetUsers.Include(a => a.Station).Where(x=>x.IsDeleted==false).Skip(rescSkip).Take(pager.PageSize).ToListAsync();
                return View(fireFighters);
            }
           
            if (User.IsInRole("Chief"))
            {
                var currentUser = _context.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();
                var users=await _context.AspNetUsers.Include(a => a.Station).Where(x=>x.StationId== currentUser.StationId).Where(x => x.IsDeleted == false).Skip(rescSkip).Take(pager.PageSize).ToListAsync();
                return View(users);
            }
            return View();

        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.AspNetUsers == null)
            {
                return NotFound();
            }

            var aspNetUser = await _context.AspNetUsers
                .Include(a => a.Station)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }
        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.AspNetUsers == null)
            {
                return NotFound();
            }

            var aspNetUser = await _userManager.FindByIdAsync(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            ViewData["Gender"] = new SelectList(_context.Genders, "Type", "Type");
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", aspNetUser.StationId);
            return View(aspNetUser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,FirstName,LastName,Gender,DateOfBirth,IsPasswordChanged,Address,StationId,IsDeleted")] User aspNetUser)
        {
            if (id != aspNetUser.Id)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            user.FirstName = aspNetUser.FirstName;
            user.LastName=aspNetUser.LastName;
            user.Gender = aspNetUser.Gender;
            user.DateOfBirth = aspNetUser.DateOfBirth;
            user.Address = aspNetUser.Address;
            user.StationId = aspNetUser.StationId;


            ViewData["Gender"] = new SelectList(_context.Genders, "Type", "Type");
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", aspNetUser.StationId);
           
            if (ModelState.IsValid)
            {
                try
                {
                    await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserExists(aspNetUser.Id))
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
           
            return View(aspNetUser);
        }  
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.AspNetUsers == null)
            {
                return Problem("Entity set 'FireFighterContext.AspNetUsers'  is null.");
            }
            //var aspNetUser = await _context.AspNetUsers.FindAsync(id);
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await _userManager.UpdateAsync(user);
            }         
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUserExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
