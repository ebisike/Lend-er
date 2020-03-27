using Lend_er.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lend_er.Data
{
    public class ApplicationUser : IdentityUser
    {
        public gender Gender { get; set; }
        public state State { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoPath { get; set; }

    }
}
