using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Models.Constants;
using Xant.MVC.Services.EmailSender;
using Xant.MVC.Services.FileUploader;
using Xant.Persistence;

namespace Xant.MVC
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IFileUploader, FileUploader>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("Default"),
                    x =>
                    {
                        x.MigrationsAssembly("Xant.Persistence");
                    }
                );
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddMvc()
                .AddRazorRuntimeCompilation();
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //Database Creation
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                var roles = new List<string>()
                {
                    ConstantUserRoles.SuperAdmin,
                    ConstantUserRoles.Admin,
                    ConstantUserRoles.Writer
                };

                User user = new User()
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    UserName = "Admin",
                    PhoneNumber = "0000000000",
                    Email = "example@gmail.com",
                    EmailConfirmed = true,
                    CreateDate = DateTime.Now,
                    IsActive = true
                };

                ApplicationDbInitializer.SeedData(context, userManager, roleManager, roles,
                    ConstantUserRoles.SuperAdmin, user, "p@sS123");
            }
        }
    }
}
