using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lend_er.Entities
{
    public class Payments
    {
        public Guid Id { get; set; }
        public Guid deptId { get; set; }
        public string userId { get; set; }
        public Deptors deptors { get; set; }
        public double TotalAmount { get; set; }
        public DateTime DatePaid { get; set; }


    }
}
