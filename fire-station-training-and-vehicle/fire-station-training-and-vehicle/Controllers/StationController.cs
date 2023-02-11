using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fire_station_training_and_vehicle.Models;
using Microsoft.AspNetCore.Authorization;
using fire_station_training_and_vehicle.Services;

namespace fire_station_training_and_vehicle.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StationController : Controller
    {
        private readonly FireFighterContext _context;

        public StationController(FireFighterContext context)
        {
            _context = context;
        }

        // GET: Station
        public async Task<IActionResult> Index(int pg = 1)
        {
            int pageSize = 9;
            if (pg < 1)
            {
                pg = 1;
            }
            int rescCount = _context.Stations.Where(x => x.IsDeleted == false).Count();
            var pager = new Pagination(rescCount, pg, pageSize);
            int rescSkip = (pg - 1) * pageSize;
            this.ViewBag.Pager = pager;
            var station=await _context.Stations.Where(x=>x.IsDeleted==false).Skip(rescSkip).Take(pager.PageSize).ToListAsync();
            return View(station);
        }

        // GET: Station/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stations == null)
            {
                return NotFound();
            }

            var station = await _context.Stations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        // GET: Station/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Station/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Email,PhoneNumber")] Station station)
        {
            if (ModelState.IsValid)
            {
                station.IsDeleted = false;
                _context.Add(station);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Station created successfully!!";
                return RedirectToAction(nameof(Index));
            }
            return View(station);
        }

        // GET: Station/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stations == null)
            {
                return NotFound();
            }

            var station = await _context.Stations.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }

        // POST: Station/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Email,IsDeleted,PhoneNumber")] Station station)
        {
            if (id != station.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(station);

                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Station edited successfully!!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationExists(station.Id))
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
            return View(station);
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Stations == null)
            {
                return Problem("Entity set 'FireFighterContext.Stations'  is null.");
            }
            var station = await _context.Stations.FindAsync(id);
            if (station != null)
            {
                station.IsDeleted=true;
                _context.Update(station);
                TempData["Success"] = "Station deleted successfully!!";

            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StationExists(int id)
        {
          return _context.Stations.Any(e => e.Id == id);
        }
    }
}
