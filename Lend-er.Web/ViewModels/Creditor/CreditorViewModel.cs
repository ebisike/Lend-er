using Lend_er.Data;
using Lend_er.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lend_er.Web.ViewModels.Creditor
{
    public class CreditorViewModel
    {
        public Guid id { get; set; }
        public string userId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Date owed")]
        public DateTime DateOwed { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        public bool status { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Money Owed")]
        public int MoneyOwed { get; set; }

        [Required]
        [Display(Name = "% interest per day")]
        public double interestPerDay { get; set; }
    }
}
