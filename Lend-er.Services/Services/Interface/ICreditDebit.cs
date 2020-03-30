using System;
using System.Collections.Generic;
using System.Text;

namespace Lend_er.Services.Services.Interface
{
    public interface ICreditDebit
    {
        public int GetAllCreditors(string id);
        public int GetAllDebitors(string id);
        public double GetAllAmountOwed(string id);
        public double GetAllAmountGained(string id);
    }
}
