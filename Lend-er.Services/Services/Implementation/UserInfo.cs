using Lend_er.Data;
using Lend_er.Services.Interface;
using Lend_er.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lend_er.Services.Services.Implementation
{
    public class UserInfo : IUserInfo
    {
        private readonly IRepository<ApplicationUser> repository;

        public UserInfo(IRepository<ApplicationUser> repository)
        {
            this.repository = repository;
        }
        ApplicationUser IUserInfo.UserInfo(string id)
        {
            return repository.GetById(id);
        }
    }
}
