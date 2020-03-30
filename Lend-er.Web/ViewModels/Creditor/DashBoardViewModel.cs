using Lend_er.Entities;

namespace Lend_er.Web.ViewModels.Creditor
{
    public class DashBoardViewModel
    {
        public Creditors creditor { get; set; }
        public int day { get; set; }
        public double total { get; set; }
    }
}