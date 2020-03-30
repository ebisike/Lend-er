using Lend_er.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lend_er.Web.ViewModels
{
    public class DashboardView
    {
        public ApplicationUser AppUser { get; set; }
        public int Debitors { get; set; }
        public int Creditors { get; set; }
    }
}
