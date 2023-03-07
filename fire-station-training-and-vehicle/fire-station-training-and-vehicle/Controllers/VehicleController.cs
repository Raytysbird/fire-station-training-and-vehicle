using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fire_station_training_and_vehicle.Models;
using System.Xml;
using Microsoft.Data.SqlClient.Server;
using Newtonsoft.Json;
using Microsoft.Build.Framework;
using Microsoft.AspNetCore.Authorization;
using static System.Collections.Specialized.BitVector32;

namespace fire_station_training_and_vehicle.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VehicleController : Controller
    {
        private readonly FireFighterContext _context;

        public VehicleController(FireFighterContext context)
        {
            _context = context;
        }

        // GET: Vehicle
        public async Task<IActionResult> Index()
        {
            var fireFighterContext = _context.Vehicles.Include(v => v.Station).Include(v => v.VehicleType).Where(x=>x.IsDeleted==false);
            return View(await fireFighterContext.ToListAsync());
        }

        [HttpGet]
        public IActionResult GetDefaultTypes(int id)
        {
            var type = _context.VehicleCatalogues.FirstOrDefault(x => x.DefaultTypeId == id);
            if (type != null)
            {
                return Json(type);
            }
            else
            {
                return NotFound();
            }

        }

        // GET: Vehicle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .Include(v => v.Station)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.VehicleId == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicle/Create
        public IActionResult Create()
        {
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name");
            ViewData["DefaultTypeId"] = new SelectList(_context.VehicleCatalogues, "DefaultTypeId", "TypeName");
            return View();
        }

        // POST: Vehicle/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleId,VehicleTypeId,StationId,Name,LicencePlate,LicenceExpiry,VehicleStatus,Make,Model,Year,IsDeleted")] Vehicle vehicle)
        {
            vehicle.VehicleStatus = "In Service";
            vehicle.IsDeleted = false;
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Vehicle created successfully!!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", vehicle.StationId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "VehicleTypeId", "VehicleTypeId", vehicle.VehicleTypeId);
            ViewData["DefaultTypeId"] = new SelectList(_context.VehicleCatalogues, "DefaultTypeId", "TypeName");
            return View(vehicle);
        }



        [HttpPost]
        public IActionResult CreateType([FromBody] FormData formData)
        {
            VehicleType type = new VehicleType();
            type.TypeId = formData.TypeId;
            type.Description = formData.Description;
            type.TankMinimumCapacity = formData.TankMinimumCapacity;
            type.HeavyRescue = formData.HeavyRescue;
            type.HoseOneHalfInch = formData.HoseOneHalfInch;
            type.HoseOneInch = formData.HoseOneInch;
            type.HoseTwoHalfInch = formData.HoseTwoHalfInch;
            type.Ladders = formData.Ladders;
            type.MasterStream = formData.MasterStream;
            type.MaximumGvr = formData.MaximumGvr;
            type.MinPersonel = formData.MinPersonel;
            type.PumpAndRoll = formData.PumpAndRoll;
            type.PumpMinimumFlow = formData.PumpMinimumFlow;
            type.RatedPressure = formData.RatedPressure;
            type.Turntable = formData.Turntable;
            type.TypicalUse = formData.TypicalUse;
            type.WildLandRescue = formData.WildLandRescue;
            type.Structure = formData.Structure;


            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                         kvp => kvp.Key,
                         kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
               );
                return Json(new { success = false, errors = errors });
            }
            else
            {
                _context.VehicleTypes.Add(type);
                _context.SaveChanges();
                return Json(new { success = true });
            }

        }

        // GET: Vehicle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", vehicle.StationId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "VehicleTypeId", "VehicleTypeId", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        // POST: Vehicle/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VehicleId,VehicleTypeId,StationId,Name,LicencePlate,LicenceExpiry,VehicleStatus,Make,Model,Year,IsDeleted")] Vehicle vehicle)
        {
            if (id != vehicle.VehicleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    TempData["Success"] = "Vehicle edited successfully!!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.VehicleId))
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
            ViewData["StationId"] = new SelectList(_context.Stations, "Id", "Name", vehicle.StationId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "VehicleTypeId", "VehicleTypeId", vehicle.VehicleTypeId);
            return View(vehicle);
        }

        

        // POST: Vehicle/Delete/5
     
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Vehicles == null)
            {
                return Problem("Entity set 'FireFighterContext.Vehicles'  is null.");
            }
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                vehicle.IsDeleted=true;
                _context.Update(vehicle);
                TempData["Success"] = "Vehicle deleted successfully!!";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.VehicleId == id);
        }
    }

    public class FormData
    {
        public int? TypeId { get; set; }
        [Required]
        public string? Description { get; set; }
        public int? TankMinimumCapacity { get; set; }
        public bool? HeavyRescue { get; set; }
        public int? HoseOneHalfInch { get; set; }
        public int? HoseOneInch { get; set; }
        public int? HoseTwoHalfInch { get; set; }
        public bool? Ladders { get; set; }
        public bool? MasterStream { get; set; }
        public int? MaximumGvr { get; set; }
        public int? MinPersonel { get; set; }
        public bool? PumpAndRoll { get; set; }
        public int? PumpMinimumFlow { get; set; }
        public int? RatedPressure { get; set; }
        public bool? Turntable { get; set; }
        public string? TypicalUse { get; set; }
        public bool? WildLandRescue { get; set; }
        public bool? Structure { get; set; }
    }
}
