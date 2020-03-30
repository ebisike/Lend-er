using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lend_er.Entities
{
    public class Deptors
    {
        public Guid Id { get; set; }
        public string userId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime DateOwed { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DatePaid { get; set; }
        public bool status { get; set; }
        public int AccountNumber { get; set; }
        public string BankName { get; set; }
        public int MoneyOwed { get; set; }
        public double percentageInterest { get; set; }
        public string Description { get; set; }
        public List<Payments> payments { get; set; }
    }
}
