using Lend_er.Data;
using Lend_er.Entities;
using Lend_er.Services.Interface;
using Lend_er.Services.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Providers.Entities;

namespace Lend_er.Services.Services.Implementation
{
    public class Notifications : INotifications
    {
        private readonly IRepository<Creditors> creditRepo;
        private readonly IRepository<Deptors> debitRepo;
        private readonly IRepository<Payments> payRepo;

        public Notifications(IRepository<Creditors> creditRepo,
                             IRepository<Deptors> debitRepo,
                             IRepository<Payments> payRepo)
        {
            this.creditRepo = creditRepo;
            this.debitRepo = debitRepo;
            this.payRepo = payRepo;
        }

        public int DueDate(string id)
        {
            DateTime today = DateTime.Today;

            return creditRepo.GetAll().Where(u => u.userId == id && u.status == false).Select(x => x.DueDate <= today).Count();
        }

        public int IncompletePaymentsCount(string id)
        {
            //double sum = payRepo.GetAll().Where(u => u.userId == id).Sum(x => x.TotalAmount);
            return debitRepo.GetAll().Where(u => u.userId == id && u.status == false).Count();
        }

        public List<Deptors> DeptorsList(string id)
        {
            //double sum = payRepo.GetAll().Where(u => u.userId == id).Sum(x => x.TotalAmount);
            return debitRepo.GetAll().Where(u => u.userId == id && u.status == false).ToList();
        }

        public double ProgressBar(Guid id)
        {
            double sum = payRepo.GetAll().Where(u => u.deptId == id).Sum(x => x.TotalAmount);
            double dept = Convert.ToDouble(debitRepo.GetAll().Where(x => x.Id == id).Select(u => u.MoneyOwed).FirstOrDefault());

            return (Math.Round((sum / dept) * 100,2));

        }
    }
}
