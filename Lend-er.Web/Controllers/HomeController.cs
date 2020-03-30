using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lend_er.Web.Models;
using Lend_er.Web.ViewModels;
using Lend_er.Data;
using Lend_er.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Lend_er.Services.Services.Interface;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Lend_er.Web.Controllers
{
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<ApplicationUser> _lenderDb;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICreditDebit creditDebit;
        private readonly IWebHostEnvironment webHostEnvironment;

        //public string userid { get; set; }

        public HomeController(ILogger<HomeController> logger,
                              IRepository<ApplicationUser> lenderDb,
                              UserManager<ApplicationUser> userManager,
                              ICreditDebit creditDebit,
                              IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _lenderDb = lenderDb;
            this.userManager = userManager;
            this.creditDebit = creditDebit;
            this.webHostEnvironment = webHostEnvironment;
        }

        public string Userid()
        {
            return userManager.GetUserId(User);
        }

        [Route("index")]
        public IActionResult Index()
        {
            ApplicationUser user = _lenderDb.GetById(Userid());
            DashboardView dashboardView = new DashboardView()
            {
                AppUser = user,
                Debitors = creditDebit.GetAllDebitors(Userid()),
                Creditors = creditDebit.GetAllCreditors(Userid())
            };
            return View(dashboardView);
            //return View();
        }

        [Route("Details/{id?}")]
        public ActionResult Details(string id)
        {
            ApplicationUser user = _lenderDb.GetById(Userid());
            DashboardView dashboardView = new DashboardView()
            {
                AppUser = user,
                Debitors = creditDebit.GetAllDebitors(Userid()),
                Creditors = creditDebit.GetAllCreditors(Userid())
            };
            return View(dashboardView);
        }

        [Route("Edit/{id?}")]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var user = _lenderDb.GetById(id);
            RegistrationViewModel model = new RegistrationViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                State = user.State,
                Gender = user.Gender,
                Email = user.Email,
                Address = user.Address,
                Id = user.Id,
            };            
            return View(model);
        }

        [Route("Edit/{id?}")]
        [HttpPost]
        public IActionResult Edit(RegistrationViewModel model)
        {
            ApplicationUser user = _lenderDb.GetById(model.Id);

            string uniqueFileName = null;
            if (model.photo != null)
            {
                string uploadfolder = Path.Combine(webHostEnvironment.WebRootPath, "ProfilePhoto");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.photo.FileName;
                string filepath = Path.Combine(uploadfolder, uniqueFileName);
                model.photo.CopyTo(new FileStream(filepath, FileMode.Create));
            }
            else
            {
                uniqueFileName = _lenderDb.GetAll().Where(u => u.Id == model.Id).Select(x => x.PhotoPath).FirstOrDefault();
            }

            
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.State = model.State;
                user.Address = model.Address;
                //user.Email = model.Email;
                user.Gender = model.Gender;
                user.PhotoPath = uniqueFileName;

                _lenderDb.Update(user);
                return RedirectToAction("Details");
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
