using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lend_er.Web.ViewModels.Deptor
{
    public class ClearDeptViewModel
    {
        public Guid Id { get; set; }
        public string userid { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }
        public int Amount { get; set; }
        public double interestRate { get; set; }
        public DateTime DatePaid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public double Pay { get; set; }

        [Display(Name = "Amount plus interest")]
        public double AmountWithInterest { get; set; }
        public double previousPay { get; set; }
        public double balance { get; set; }
    }
}
