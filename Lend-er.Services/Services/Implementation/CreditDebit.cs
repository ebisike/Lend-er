using Lend_er.Data;
using Lend_er.Entities;
using Lend_er.Services.Interface;
using Lend_er.Services.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lend_er.Services.Services.Implementation
{
    public class CreditDebit : ICreditDebit
    {
        private readonly IRepository<Creditors> creditRepo;
        private readonly IRepository<Deptors> debitRepo;

        public CreditDebit(IRepository<Creditors> creditRepo,
                           IRepository<Deptors> debitRepo)
        {
            this.creditRepo = creditRepo;
            this.debitRepo = debitRepo;
        }

        public double GetAllAmountGained(string id)
        {
            return creditRepo.GetAll().Where(user => user.userId == id && user.status == false).Sum(x => x.MoneyOwed);
        }

        public double GetAllAmountOwed(string id)
        {
            return debitRepo.GetAll().Where(u => u.userId == id && u.status == false).Select(c => c.MoneyOwed).Sum();
        }

        public int GetAllCreditors(string id)
        {
            //this code uses the current user id to get the total count of all creditors
            return creditRepo.GetAll().Where(user => user.userId == id && user.status == false).Count();
        }

        public int GetAllDebitors(string id)
        {
            //this code uses the current user id to get the total count of all deptors
            return debitRepo.GetAll().Where(user => user.userId == id && user.status == false).Count();
        }
        public int GetJan(string id)
        {
            //int jan = 1;
            return creditRepo.GetAll().Where(user => user.userId == id && user.status == false).Select(x => x.DateOwed.Month <= 6).Count();
        }
    }
}
