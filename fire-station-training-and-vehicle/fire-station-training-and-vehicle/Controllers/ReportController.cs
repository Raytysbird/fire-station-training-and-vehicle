using fire_station_training_and_vehicle.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace fire_station_training_and_vehicle.Controllers
{
    public class ReportController : Controller
    {
        private readonly FireFighterContext _context;
        private readonly UserManager<User> _userManager;
        public ReportController(FireFighterContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "Name");
            return View();
        }
        public IActionResult UserReport()
        {

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                int count = 0;
                worksheet.Cell(currentRow, 1).Value = "User Name";
                worksheet.Cell(currentRow, 2).Value = "Email";
                worksheet.Cell(currentRow, 3).Value = "Full Name";
                worksheet.Cell(currentRow, 4).Value = "Date Of Birth";
                worksheet.Cell(currentRow, 5).Value = "Address";


                var users = _context.AspNetUsers;
                foreach (var item in users)
                {
                    currentRow++;
                    count++;
                    worksheet.Cell(currentRow, 1).Value = item.UserName;
                    worksheet.Cell(currentRow, 2).Value = item.Email;
                    worksheet.Cell(currentRow, 3).Value = item.FirstName + " " + item.LastName;
                    worksheet.Cell(currentRow, 4).Value = item.DateOfBirth;
                    worksheet.Cell(currentRow, 5).Value = item.Address;


                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "UserList.xlsx"
                        );
                }

            }
        }


        public IActionResult StationReport()
        {

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Stations");
                var currentRow = 1;
                int count = 0;
                worksheet.Cell(currentRow, 1).Value = "Name";
                worksheet.Cell(currentRow, 2).Value = "Address";
                worksheet.Cell(currentRow, 3).Value = "Email";
                worksheet.Cell(currentRow, 4).Value = "Phone";


                var users = _context.Stations;
                foreach (var item in users)
                {
                    currentRow++;
                    count++;
                    worksheet.Cell(currentRow, 1).Value = item.Name;
                    worksheet.Cell(currentRow, 2).Value = item.Address;
                    worksheet.Cell(currentRow, 3).Value = item.Email;
                    worksheet.Cell(currentRow, 4).Value = item.PhoneNumber;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "StationsList.xlsx"
                        );
                }

            }

        }
        public IActionResult AllVehiclesList()
        {

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Vehicles");
                var currentRow = 1;
                int count = 0;
                worksheet.Cell(currentRow, 1).Value = "Name";
                worksheet.Cell(currentRow, 2).Value = "Licence Plate";
                worksheet.Cell(currentRow, 3).Value = "Licence Expiry";
                worksheet.Cell(currentRow, 4).Value = "Make";
                worksheet.Cell(currentRow, 5).Value = "Model";
                worksheet.Cell(currentRow, 6).Value = "Year";
                worksheet.Cell(currentRow, 7).Value = "Status";


                var users = _context.Vehicles.Include(x => x.VehicleType);
                foreach (var item in users)
                {
                    currentRow++;
                    count++;
                    worksheet.Cell(currentRow, 1).Value = item.Name;
                    worksheet.Cell(currentRow, 2).Value = item.LicencePlate;
                    worksheet.Cell(currentRow, 3).Value = item.LicenceExpiry;
                    worksheet.Cell(currentRow, 4).Value = item.Make;
                    worksheet.Cell(currentRow, 5).Value = item.Model;
                    worksheet.Cell(currentRow, 6).Value = item.Year;
                    worksheet.Cell(currentRow, 7).Value = item.VehicleStatus;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "VehicleList.xlsx"
                        );
                }

            }

        }
        public IActionResult MaintenanceRecord(Vehicle vehicle)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("MaintenanceRecord");
                var currentRow = 1;
                int count = 0;
                worksheet.Cell(currentRow, 1).Value = "Name";
                worksheet.Cell(currentRow, 2).Value = "Date of Repair";
                worksheet.Cell(currentRow, 3).Value = "Description";
                worksheet.Cell(currentRow, 4).Value = "Mieleage";
                worksheet.Cell(currentRow, 5).Value = "Special Notes";
              
                var record = _context.Maintenances.Include(x => x.Vehicle).Where(x => x.VehicleId == vehicle.VehicleId).ToList();
                foreach (var item in record)
                {
                    currentRow++;
                    count++;
                    worksheet.Cell(currentRow, 1).Value = item.Vehicle.Name;
                    worksheet.Cell(currentRow, 2).Value = item.DateOfRepair;
                    worksheet.Cell(currentRow, 3).Value = item.Description;
                    worksheet.Cell(currentRow, 4).Value = item.Milage;
                    worksheet.Cell(currentRow, 5).Value = item.Notes;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "MaintenanceRecord.xlsx"
                        );
                }

            }

        }
        public IActionResult VehicleReport(Vehicle vehicle)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("VehicleReports");
                var currentRow = 1;
                int count = 0;
                worksheet.Cell(currentRow, 1).Value = "Name";
                worksheet.Cell(currentRow, 2).Value = "User";
                worksheet.Cell(currentRow, 3).Value = "Reported Date";
                worksheet.Cell(currentRow, 4).Value = "Status";
                worksheet.Cell(currentRow, 5).Value = "Issue";
                worksheet.Cell(currentRow, 6).Value = "Description";

                var record = _context.VehicleReports.Include(x => x.Vehicle).Include(x=>x.User).Where(x => x.VehicleId == vehicle.VehicleId).ToList();
                foreach (var item in record)
                {
                    currentRow++;
                    count++;
                    worksheet.Cell(currentRow, 1).Value = item.Vehicle.Name;
                    worksheet.Cell(currentRow, 2).Value = item.User.FirstName+" "+item.User.LastName;
                    worksheet.Cell(currentRow, 3).Value = item.DateReported;
                    worksheet.Cell(currentRow, 4).Value = item.Status;
                    worksheet.Cell(currentRow, 5).Value = item.IssueType;
                    worksheet.Cell(currentRow, 6).Value = item.Description;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "VehicleReport.xlsx"
                        );
                }

            }

        }

    }
}
