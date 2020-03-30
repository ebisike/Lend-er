using Lend_er.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lend_er.Web.ViewModels.Deptor
{
    public class DashboardViewModel
    {
        public Deptors deptors { get; set; }
        public int day { get; set; }
        public double total { get; set; }
        public double MoneyPaid { get; set; }
        public double balance { get; set; }
    }
}
