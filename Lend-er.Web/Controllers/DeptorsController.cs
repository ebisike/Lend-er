using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lend_er.Data;
using Lend_er.Entities;
using Lend_er.Services.Interface;
using Lend_er.Web.ViewModels.Debitor;
using Lend_er.Web.ViewModels.Deptor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lend_er.Web.Controllers
{
    [Route("Debitors")]
    public class DeptorsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<ApplicationUser> lenderDb;
        private readonly IRepository<Deptors> debitRepo;
        private readonly IRepository<Payments> payRepo;

        public DeptorsController(UserManager<ApplicationUser> userManager,
                                   IRepository<ApplicationUser> lenderDb,
                                   IRepository<Deptors> debitRepo,
                                   IRepository<Payments> payRepo)
        {
            this.userManager = userManager;
            this.lenderDb = lenderDb;
            this.debitRepo = debitRepo;
            this.payRepo = payRepo;
        }

        public string Userid()
        {
            return userManager.GetUserId(User);
        }

        // GET: Debitors
        [Route("index")]
        public ActionResult Index()
        {
            var model = debitRepo.GetAll().Where(u => u.userId == Userid()).ToList();
            return View(model);
        }

        //Get: Active deptors
        [Route("active")]
        public ActionResult Active()
        {
            var model = debitRepo.GetAll().Where(u => u.userId == Userid() && u.status == false).ToList();
            return View(model);
        }

        //Get: Inactive deptors
        [Route("inactive")]
        public ActionResult Inactive()
        {
            var model = debitRepo.GetAll().Where(u => u.userId == Userid() && u.status == true).ToList();
            return View(model);
        }

        // GET: deptors/Details/5
        [Route("Details/{id?}")]
        public ActionResult Details(Guid id)
        {
            Deptors deptors = debitRepo.GetByIdCreditDebit(id);
            int day1 = deptors.DateOwed.Day;
            int day2 = deptors.DueDate.Day;
            int day = day2 - day1;

            //calculate interest
            double interest = deptors.percentageInterest * day;

            double pay = payRepo.GetAll().Where(u => u.userId == Userid() && u.deptId == id).Sum(x => x.TotalAmount);

            DashboardViewModel model = new DashboardViewModel()
            {
                deptors = deptors,
                total = (interest * deptors.MoneyOwed) + deptors.MoneyOwed,
                day = day,
                MoneyPaid = pay,
                balance = deptors.MoneyOwed - pay
            };
            return View(model);
        }

        // GET: deptors/Create
        [Route("create")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        // POST: deptors/Create
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DebitorViewModel model)
        {
            if (ModelState.IsValid)
            {
                Deptors deptors = new Deptors()
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
                    AccountNumber = model.AccountNumber,
                    BankName = model.BankName,
                    MoneyOwed = model.MoneyOwed,

                    userId = Userid()
                };
                debitRepo.Insert(deptors);
                return RedirectToAction("index", "Deptors");
            }
            return View();
        }

        // GET: deptors/Edit/
        [Route("edit/{id?}")]
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            Deptors deptors = debitRepo.GetByIdCreditDebit(id);
            DebitorViewModel model = new DebitorViewModel()
            {
                id = deptors.Id,
                userId = deptors.userId,
                FirstName = deptors.FirstName,
                LastName = deptors.LastName,
                Address = deptors.Address,
                Phone = deptors.Phone,
                Email = deptors.Email,
                Description = deptors.Description,
                DueDate = deptors.DueDate,
                interestPerDay = deptors.percentageInterest,
                MoneyOwed = deptors.MoneyOwed,
                AccountNumber = deptors.AccountNumber,
                BankName = deptors.BankName
            };
            return View(model);
        }

        // POST: deptors/Edit/5
        [Route("edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DebitorViewModel model)
        {
            Deptors deptors = debitRepo.GetByIdCreditDebit(model.id);
            if (ModelState.IsValid)
            {
                deptors.FirstName = model.FirstName;
                deptors.LastName = model.LastName;
                deptors.Address = model.Address;
                deptors.MoneyOwed = model.MoneyOwed;
                deptors.percentageInterest = model.interestPerDay;
                deptors.Description = model.Description;
                deptors.Phone = model.Phone;
                deptors.Email = model.Email;
                deptors.DueDate = model.DueDate;
                deptors.AccountNumber = model.AccountNumber;
                deptors.BankName = model.BankName;

                debitRepo.Update(deptors);
                return RedirectToAction("index");
            }
            return View();
        }

        // GET: Creditors/Delete/5
        [Route("delete/{id?}")]
        public ActionResult Delete(Guid id)
        {
            debitRepo.DeleteCreditDebit(id);
            return RedirectToAction("index");
        }

        
        //Pay offline
        [Route("payoffline/{id?}")]
        [HttpGet]
        public ActionResult PayOffline(Guid id)
        {
            Deptors deptors = debitRepo.GetByIdCreditDebit(id);
            double pay = payRepo.GetAll().Where(x => x.deptId == id).Sum(a => a.TotalAmount);
            double amountInterested = (deptors.MoneyOwed * deptors.percentageInterest) + deptors.MoneyOwed;
            ClearDeptViewModel model = new ClearDeptViewModel()
            {
                Id = deptors.Id,
                userid = deptors.userId,
                BankName = deptors.BankName,
                AccountNumber = deptors.AccountNumber,
                interestRate = deptors.percentageInterest,
                Amount = deptors.MoneyOwed,
                AmountWithInterest = amountInterested,
                FirstName = deptors.FirstName,
                LastName = deptors.LastName,
                previousPay = pay,
                balance = deptors.MoneyOwed - pay
            };

            return View(model);
        }

        //Pay offline
        [Route("payoffline/{id?}")]
        [HttpPost]
        public IActionResult PayOffline(ClearDeptViewModel model)
        {
            //get the instance of the deptor
            Deptors deptors = debitRepo.GetByIdCreditDebit(model.Id);

            //get the sum of all previous payments
            double previousPay = payRepo.GetAll().Where(u => u.deptId == model.Id && u.userId == model.userid).Sum(x => x.TotalAmount);
            

            if (ModelState.IsValid)
            {
                if ((model.Pay + previousPay) >= deptors.MoneyOwed)
                {
                    deptors.status = true;
                    Payments pay = new Payments()
                    {
                        deptId = model.Id,
                        userId = model.userid,
                        TotalAmount = model.Pay
                    };
                    
                    debitRepo.Update(deptors);
                    payRepo.Insert(pay);
                    return RedirectToAction("index");
                }
                else
                {
                    Payments pay = new Payments()
                    {
                        deptId = model.Id,
                        userId = model.userid,
                        TotalAmount = model.Pay
                    };                

                    payRepo.Insert(pay);
                    return RedirectToAction("index");
                }

                
            }

            return View();
        }
    }
}