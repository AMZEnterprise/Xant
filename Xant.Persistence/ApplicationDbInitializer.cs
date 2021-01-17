using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using Xant.Core.Domain;

namespace Xant.Persistence
{
    /// <summary>
    /// Represents application database initializer interface 
    /// </summary>
    public class ApplicationDbInitializer
    {
        private static ApplicationDbContext _context;
        private static List<string> _roleNames = new List<string>();
        private static string _superUserRole = string.Empty;
        private static User _user = new User();
        private static string _userPassword = string.Empty;

        public static void SeedData(
            ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            List<string> roleNames,
            string superUserRole,
            User user,
            string userPassword
            )
        {
            _context = context;
            _roleNames = roleNames;
            _superUserRole = superUserRole;
            _user = user;
            _userPassword = userPassword;
            SeedRoles(roleManager);
            SeedUsers(userManager);
            SeedDatabase();
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in _roleNames)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    IdentityResult roleResult = roleManager.
                        CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<User> userManager)
        {
            var adminCount = userManager.GetUsersInRoleAsync(_superUserRole).Result.Count;

            if (adminCount <= 0)
            {
                if (userManager.FindByNameAsync
                    (_user.UserName).Result == null)
                {
                    var result = userManager.CreateAsync
                        (_user, _userPassword).Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(_user, _superUserRole).Wait();
                    }
                }
            }

        }

        private static void SeedDatabase()
        {
            if (!_context.Settings.Any())
            {
                _context.Settings.Add(new Setting());
                _context.SaveChanges();
            }
        }
    }
}