using Lend_er.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lend_er.Services.Services.Interface
{
    public interface INotifications
    {
        public int DueDate(string id);
        public int IncompletePaymentsCount(string id);
        public List<Deptors> DeptorsList(string id);
        public double ProgressBar(Guid id);
    }
}
