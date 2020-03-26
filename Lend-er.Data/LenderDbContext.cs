using Lend_er.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lend_er.Data
{
    public class LenderDbContext : DbContext
    {
        public LenderDbContext(DbContextOptions<LenderDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Deptors> deptors { get; set; }
        public DbSet<Creditors> creditors { get; set; }
    }
}
