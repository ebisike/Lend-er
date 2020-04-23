using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lend_er.Data;
using Lend_er.Entities;
using Lend_er.Services.Implementation;
using Lend_er.Services.Interface;
using Lend_er.Services.Services.Implementation;
using Lend_er.Services.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lend_er.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContextPool<LenderDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LenderConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            }).AddEntityFrameworkStores<LenderDbContext>().AddDefaultTokenProviders();

            services.AddMvc(opt => opt.EnableEndpointRouting = false);
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICreditDebit, CreditDebit>();
            services.AddScoped<IUserInfo, UserInfo>();
            services.AddScoped<INotifications, Notifications>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            //app.UseRouting();
            app.UseMvc(routes=> 
            {
                routes.MapRoute("default", "{controller=Account}/{action=login}/{id?}");
            });

            app.UseAuthorization();            
        }
    }
}
