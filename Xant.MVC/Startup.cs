using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartBreadcrumbs.Extensions;
using System;
using System.Collections.Generic;
using ElmahCore;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Xant.Core;
using Xant.Core.Domain;
using Xant.MVC.Models.Constants;
using Xant.MVC.Services.EmailSender;
using Xant.MVC.Services.FileHandler;
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
            services.AddTransient<IFileHandler, FileHandler>();

            //Elmah Error Logger
            services.AddElmah<XmlFileErrorLog>(options =>
            {
                options.Path = "/Panel/elmah";
                options.LogPath = "~/logs";
            });

            services.Configure<EmailSenderOptions>(options =>
                Configuration.GetSection("EmailService").Bind(options));

            //Breadcrumbs NavigationDefault (Bootstrap 4.1) CSS
            services.AddBreadcrumbs(GetType().Assembly, options =>
            {
                options.TagName = "nav";
                options.TagClasses = "";
                options.OlClasses = "breadcrumb d-flex justify-content-center";
                options.LiClasses = "breadcrumb-item";
                options.ActiveLiClasses = "breadcrumb-item active";
            });

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

            services.AddIdentity<User, IdentityRole>(
                    options =>
                    {
                        options.Lockout.AllowedForNewUsers = true;
                        options.Lockout.MaxFailedAccessAttempts = 3;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequiredLength = 6;
                        options.SignIn.RequireConfirmedPhoneNumber = true;
                    }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddAutoMapper(typeof(Startup));

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddMvc()
                .AddNewtonsoftJson()
                .AddRazorRuntimeCompilation();

            services.ConfigureApplicationCookie(options => options.LoginPath = "/Panel/Users/Login");
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

            app.UseElmah();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapAreaControllerRoute(
                    areaName: "Panel",
                    name: "panel",
                    pattern: "panel/{controller=Home}/{action=Index}/{id?}");

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

                var user = new User()
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    UserName = "A@dmin13",
                    Email = "example@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true,
                    FilesPathGuid = Guid.NewGuid(),
                    CreateDate = DateTime.Now,
                    LastEditDate = DateTime.Now
                };

                ApplicationDbInitializer.SeedData(context, userManager, roleManager, roles,
                    ConstantUserRoles.SuperAdmin, user, "pssdfh@23");
            }
        }
    }
}
