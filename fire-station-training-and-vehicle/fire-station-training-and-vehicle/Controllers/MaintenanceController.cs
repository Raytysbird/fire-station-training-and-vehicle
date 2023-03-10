using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fire_station_training_and_vehicle.Models;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Composition;

namespace fire_station_training_and_vehicle.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly FireFighterContext _context;

        public MaintenanceController(FireFighterContext context)
        {
            _context = context;
        }

        // GET: Maintenance
        public IActionResult Index()
        {  
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "Name");
            return View();
        }
        public async Task<IActionResult> NotInService()
        {
           var vehicle= await _context.Maintenances.Include(x=>x.Vehicle).Where(x=>x.Status=="Out").ToListAsync();
            return View(vehicle);
        }

        public async Task<IActionResult> MarkResolve(int id)
        {
            var repair = _context.Maintenances.Find(id);

            repair.DateCompleted = DateTime.Now;
            repair.Status = "In";
            _context.Update(repair);
            await _context.SaveChangesAsync();

            var vehicle = _context.Vehicles.Find(repair.VehicleId);
            vehicle.VehicleStatus = "In Service";
            _context.Update(repair);

            await _context.SaveChangesAsync();

            return RedirectToAction("NotInService", "Maintenance");


        }
        public IActionResult MaintenanceHome()
        {
            int totalVehicles = _context.Vehicles.Count();
            var vehicles = _context.Vehicles.Where(x => x.VehicleStatus == "In Service");
            int inService = vehicles.Count();
            int outVehicles = totalVehicles - inService;

            ViewBag.TotalVehicles = totalVehicles;
            ViewBag.InService = inService;
            ViewBag.OutVehicles = outVehicles;

            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "Name");
            return View();
        }
        [HttpGet]
        public IActionResult MaintenanceRecord(int id)
        {
          
            var record=  _context.Maintenances.Include(x=>x.Vehicle).Where(x=>x.VehicleId == id).ToList();
           
            return Ok(record);
        }

        // GET: Maintenance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Maintenances == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .FirstOrDefaultAsync(m => m.RepairId == id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // GET: Maintenance/Create
        public IActionResult Create()
        {
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "Name");
            ViewData["IssueType"] = new SelectList(_context.IssueTypes, "Issue", "Issue");
            return View();
        }

        // POST: Maintenance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RepairId,VehicleId,DateOfRepair,Description,DateCompleted,Milage,Notes,Status")] Maintenance maintenance)
        {
            maintenance.DateOfRepair= DateTime.Now;
            maintenance.Status = "Out";
            var vehicle = _context.Vehicles.Find(maintenance.VehicleId);
            if (vehicle != null)
            {
                vehicle.VehicleStatus = "Out for maintenance";
                _context.Update(vehicle);

            }
            if (ModelState.IsValid)
            {
                _context.Add(maintenance);
                await _context.SaveChangesAsync();
                return RedirectToAction("MaintenanceHome");
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "Name", maintenance.VehicleId);
            ViewData["IssueType"] = new SelectList(_context.IssueTypes, "Issue", "Issue");
            return View(maintenance);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRecord([FromBody] MaintenanceData formData)
        {
            Maintenance maintenance = new Maintenance();

            maintenance.DateOfRepair = DateTime.Now;
            maintenance.Description = formData.Description;
            maintenance.Milage = formData.Milage;
            maintenance.Notes = formData.Notes;
            maintenance.VehicleId = formData.VehicleId;
            maintenance.Status = "Out";
            var reportId = formData.ReportId;
            var report = _context.VehicleReports.Find(reportId);
            if (report != null)
            {
                report.Status = "Action taken";
                _context.Update(report);

            }
            var vehicle = _context.Vehicles.Find(maintenance.VehicleId);
            if (vehicle != null)
            {
                vehicle.VehicleStatus = "Out for maintenance";
                _context.Update(vehicle);

            }
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
                _context.Add(maintenance);
               await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

        }

        // GET: Maintenance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Maintenances == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null)
            {
                return NotFound();
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "VehicleId", maintenance.VehicleId);
            return View(maintenance);
        }

        // POST: Maintenance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RepairId,VehicleId,DateOfRepair,Description,DateCompleted,Milage,Notes,Status")] Maintenance maintenance)
        {
            if (id != maintenance.RepairId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maintenance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceExists(maintenance.RepairId))
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
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "VehicleId", maintenance.VehicleId);
            return View(maintenance);
        }

        // GET: Maintenance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Maintenances == null)
            {
                return NotFound();
            }

            var maintenance = await _context.Maintenances
                .Include(m => m.Vehicle)
                .FirstOrDefaultAsync(m => m.RepairId == id);
            if (maintenance == null)
            {
                return NotFound();
            }

            return View(maintenance);
        }

        // POST: Maintenance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Maintenances == null)
            {
                return Problem("Entity set 'FireFighterContext.Maintenances'  is null.");
            }
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance != null)
            {
                _context.Maintenances.Remove(maintenance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceExists(int id)
        {
            return _context.Maintenances.Any(e => e.RepairId == id);
        }
    }
    public class MaintenanceData
    {
        public int RepairId { get; set; }
        public int ReportId { get; set; }
        public int VehicleId { get; set; }
        public DateTime? DateOfRepair { get; set; }
        public string? Description { get; set; }
        public DateTime? DateCompleted { get; set; }
        public int? Milage { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }

    }
}
