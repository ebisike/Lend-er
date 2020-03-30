using Lend_er.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lend_er.Services.Services.Interface
{
    public interface IUserInfo
    {
        ApplicationUser UserInfo(string id);
    }
}
