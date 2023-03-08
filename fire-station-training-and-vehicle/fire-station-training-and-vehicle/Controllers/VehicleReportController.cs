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
    public class VehicleReportController : Controller
    {
        private readonly FireFighterContext _context;
        private readonly UserManager<User> _userManager;

        public VehicleReportController(FireFighterContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: VehicleReport
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var fireFighterContext = _context.VehicleReports.Include(v => v.User).Include(v => v.Vehicle).Where(x=>x.UserId==userId);
            return View(await fireFighterContext.ToListAsync());
        }

     

        // GET: VehicleReport/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = _context.AspNetUsers.FirstOrDefault(x=>x.Id==userId);
            var vehicles=_context.Vehicles.Include(x=>x.Station).ThenInclude(x=>x.AspNetUsers).Where(x=>x.StationId==user.StationId).Where(x=>x.IsDeleted==false);
            ViewData["VehicleId"] = new SelectList(vehicles, "VehicleId", "Name");
            ViewData["IssueType"] = new SelectList(_context.IssueTypes, "Issue", "Issue");
            return View();
        }

        // POST: VehicleReport/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,UserId,VehicleId,DateReported,Status,IssueType,Description")] VehicleReport vehicleReport)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (ModelState.IsValid)
            {
                vehicleReport.UserId = userId;
                vehicleReport.Status = "In Review";
                _context.Add(vehicleReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", vehicleReport.UserId);
            
            var user = _context.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            var vehicles = _context.Vehicles.Include(x => x.Station).ThenInclude(x => x.AspNetUsers).Where(x => x.StationId == user.StationId).Where(x => x.IsDeleted == false);
            ViewData["VehicleId"] = new SelectList(vehicles, "VehicleId", "Name");
            ViewData["IssueType"] = new SelectList(_context.IssueTypes, "Issue", "Issue");
            return View(vehicleReport);
        }

        // GET: VehicleReport/Edit/5
        public async Task<IActionResult> Issues()
        {
            var issues=await _context.VehicleReports.Include(v => v.User).Include(v => v.Vehicle).Where(x=>x.Status== "In Review").ToListAsync();
            return View(issues);
        
        }
      
    }
}
