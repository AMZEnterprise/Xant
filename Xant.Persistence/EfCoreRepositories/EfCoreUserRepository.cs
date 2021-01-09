using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xant.Core.Domain;
using Xant.Core.Repositories;

namespace Xant.Persistence.EfCoreRepositories
{
    public class EfCoreUserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public EfCoreUserRepository(
            UserManager<User> userManager,
            SignInManager<User> singInManager,
            IPasswordHasher<User> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = singInManager;
            _passwordHasher = passwordHasher;
        }

        public IQueryable<User> GetAll()
        {
            return _userManager.Users;
        }

        public async Task<User> GetByClaimsPrincipal(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task<bool> IsInRole(User user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<User> FindByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task SignIn(User user, bool isPersistent)
        {
            await _signInManager.SignInAsync(user, isPersistent);
        }

        public async Task<SignInResult> SignInByPassword(User user, string password, bool isPersistent, bool lookoutOnFailure)
        {
            return await _signInManager.PasswordSignInAsync(user, password, isPersistent, lookoutOnFailure);
        }

        public async Task<string> GenerateChangePhoneNumberToken(User user, string phoneNumber)
        {
            return await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        }

        public async Task<bool> VerifyChangePhoneNumberToken(User user, string token, string phoneNumber)
        {
            return await _userManager.VerifyChangePhoneNumberTokenAsync(user, token, phoneNumber);
        }

        public async Task<IdentityResult> ChangePassword(User user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IdentityResult> ChangePhoneNumber(User user, string phoneNumber, string token)
        {
            return await _userManager.ChangePhoneNumberAsync(user, phoneNumber, token);
        }

        public async Task<IdentityResult> RemovePassword(User user)
        {
            return await _userManager.RemovePasswordAsync(user);
        }

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public async Task<IdentityResult> Update(User user)
        {
            user.LastEditDate = DateTime.Now;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<User> FindById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IList<string>> GetRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> Insert(User user, string password)
        {
            user.CreateDate = user.LastEditDate = DateTime.Now;

            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddToRole(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<int> Count()
        {
            return await _userManager.Users.CountAsync();
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<string> GeneratePasswordResetToken(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPassword(User user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
    }
}
