using fire_station_training_and_vehicle.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace fire_station_training_and_vehicle.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly FireFighterContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, FireFighterContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var id = _userManager.GetUserId(HttpContext.User);
            var user =  _context.AspNetUsers.Where(x=>x.Id== id).FirstOrDefault();
            var isTeacher = _context.Events.Where(x => x.TeacherId == id && x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).ToList();
            if (isTeacher.Count>0)
            {
                ViewBag.IsTeacher = true;
            }

            if (user.IsPasswordChanged==false)
            {
                TempData["Warning"] = "We have provided you one time use passowrd. Please reset your passowrd.";
                return RedirectToPage("/Account/Manage/ChangePassword", new { area = "Identity" });
            }
            if (id!=null)
            {
                return View();
            }
            else
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            }
           
            
        }

        public IActionResult Privacy()
        {
           
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}