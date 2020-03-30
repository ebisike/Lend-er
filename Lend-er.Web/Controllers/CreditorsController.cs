using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lend_er.Data;
using Lend_er.Entities;
using Lend_er.Services.Interface;
using Lend_er.Web.ViewModels.Creditor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lend_er.Web.Controllers
{
    [Route("Creditors")]
    public class CreditorsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<ApplicationUser> lenderDb;
        private readonly IRepository<Creditors> creditorRepo;

        public CreditorsController(UserManager<ApplicationUser> userManager,
                                   IRepository<ApplicationUser> lenderDb,
                                   IRepository<Creditors> creditorRepo)
        {
            this.userManager = userManager;
            this.lenderDb = lenderDb;
            this.creditorRepo = creditorRepo;
        }

        public string Userid()
        {
            return userManager.GetUserId(User);
        }
        // GET: Creditors
        [Route("index")]
        public ActionResult Index()
        {
            var model = creditorRepo.GetAll().Where(u => u.userId == Userid()).ToList();
            return View(model);
        }

        //Get: Active creditors
        [Route("active")]
        public ActionResult Active()
        {
            var model = creditorRepo.GetAll().Where(u => u.userId == Userid() && u.status == false).ToList();
            return View(model);
        }

        //Get: Inactive creditors
        [Route("inactive")]
        public ActionResult Inactive()
        {
            var model = creditorRepo.GetAll().Where(u => u.userId == Userid() && u.status == true).ToList();
            return View(model);
        }

        // GET: Creditors/Details/5
        [Route("Details/{id?}")]
        public ActionResult Details(Guid id)
        {
            Creditors creditors = creditorRepo.GetByIdCreditDebit(id);
            int day1 = creditors.DateOwed.Day;
            int day2 = creditors.DueDate.Day;
            int day = day2 - day1;

            //calculate interest
            double interest = creditors.percentageInterest * day;
            
            DashBoardViewModel model = new DashBoardViewModel()
            {
                creditor = creditors,
                total = (interest * creditors.MoneyOwed) + creditors.MoneyOwed,
                day = day
            };
            return View(model);
        }

        // GET: Creditors/Create
        [Route("create")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Creditors/Create
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreditorViewModel model)
        {
            if (ModelState.IsValid)
            {
                Creditors creditors = new Creditors()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    Address = model.Address,
                    Description = model.Description,
                    DateOwed = model.DateOwed,
                    DueDate = model.DueDate,
                    percentageInterest = model.interestPerDay,
                    status = false,
                    Email = model.Email,
                    MoneyOwed = model.MoneyOwed,
                    userId = Userid()
                };
                creditorRepo.Insert(creditors);
                return RedirectToAction("index", "Creditors");
            }
            return View();
        }

        // GET: Creditors/Edit/
        [Route("edit/{id?}")]
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            Creditors creditors = creditorRepo.GetByIdCreditDebit(id);
            CreditorViewModel model = new CreditorViewModel()
            {
                id = creditors.Id,
                userId = creditors.userId,
                FirstName = creditors.FirstName,
                LastName = creditors.LastName,
                Address = creditors.Address,
                Phone = creditors.Phone,
                Email = creditors.Email,
                Description = creditors.Description,
                DueDate = creditors.DueDate,
                interestPerDay = creditors.percentageInterest,
                MoneyOwed = creditors.MoneyOwed
            };
            return View(model);
        }

        // POST: Creditors/Edit/5
        [Route("edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CreditorViewModel model)
        {
            Creditors creditors = creditorRepo.GetByIdCreditDebit(model.id);
            if (ModelState.IsValid)
            {
                creditors.FirstName = model.FirstName;
                creditors.LastName = model.LastName;
                creditors.Address = model.Address;
                creditors.MoneyOwed = model.MoneyOwed;
                creditors.percentageInterest = model.interestPerDay;
                creditors.Description = model.Description;
                creditors.Phone = model.Phone;
                creditors.Email = model.Email;
                creditors.DueDate = model.DueDate;

                creditorRepo.Update(creditors);
                return RedirectToAction("index");
            }
            return View();
        }

        [Route("claim/{id?}")]
        public IActionResult Claim(Guid id)
        {
            Creditors creditors = creditorRepo.GetByIdCreditDebit(id);
            creditors.status = true;
            creditorRepo.Update(creditors);
            return RedirectToAction("index", "Home");
        }

        // GET: Creditors/Delete/5
        [Route("delete/{id?}")]
        public ActionResult Delete(Guid id)
        {
            creditorRepo.DeleteCreditDebit(id);
            return RedirectToAction("index");
        }

        // POST: Creditors/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}