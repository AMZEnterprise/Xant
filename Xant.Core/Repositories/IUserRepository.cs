using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xant.Core.Domain;

namespace Xant.Core.Repositories
{
    /// <summary>
    /// Represents user repository interface 
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>returns all users</returns>
        IQueryable<User> GetAll();
        /// <summary>
        /// Get user by ClaimsPrincipal
        /// </summary>
        /// <param name="principal">claims principal</param>
        /// <returns>returns user with specific claims principal</returns>
        Task<User> GetByClaimsPrincipal(ClaimsPrincipal principal);
        /// <summary>
        /// Specify if user has a role
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="role">role name</param>
        /// <returns>returns true if user is in the role or false if not</returns>
        Task<bool> IsInRole(User user, string role);
        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="userName">user name</param>
        /// <returns>returns user with specific username</returns>
        Task<User> FindByUserName(string userName);
        /// <summary>
        /// Sign out current user
        /// </summary>
        /// <returns></returns>
        Task SignOut();
        /// <summary>
        /// Sign in user
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="isPersistent">is user persistent</param>
        /// <returns></returns>
        Task SignIn(User user, bool isPersistent);
        /// <summary>
        /// Sign in user with password
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="password">user password</param>
        /// <param name="isPersistent">is user persistent</param>
        /// <param name="lookoutOnFailure">has look out on failure</param>
        /// <returns>returns SignInResult</returns>
        Task<SignInResult> SignInByPassword(User user, string password, bool isPersistent, bool lookoutOnFailure);
        /// <summary>
        /// Generate change user phone number token
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="phoneNumber">user phone number</param>
        /// <returns>returns phone number change token</returns>
        Task<string> GenerateChangePhoneNumberToken(User user, string phoneNumber);
        /// <summary>
        /// Verify change user phone number validity
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="token">user change phone number token</param>
        /// <param name="phoneNumber">user phone number</param>
        /// <returns></returns>
        Task<bool> VerifyChangePhoneNumberToken(User user, string token, string phoneNumber);
        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="currentPassword">user current password</param>
        /// <param name="newPassword">user new password</param>
        /// <returns>returns IdentityResult</returns>
        Task<IdentityResult> ChangePassword(User user, string currentPassword, string newPassword);
        /// <summary>
        /// Change user phone nubmer
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="phoneNumber">user phone number</param>
        /// <param name="token">user change phone number token</param>
        /// <returns>returns IdentityResult</returns>
        Task<IdentityResult> ChangePhoneNumber(User user, string phoneNumber, string token);
        /// <summary>
        /// Remove user password
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>returns IdentityResult</returns>
        Task<IdentityResult> RemovePassword(User user);
        /// <summary>
        /// Hash user password
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="password">user password</param>
        /// <returns>returns user password hash</returns>
        string HashPassword(User user, string password);
        /// <summary>
        /// Update existing user
        /// </summary>
        /// <param name="user">updated user</param>
        /// <returns>returns IdentityResult</returns>
        Task<IdentityResult> Update(User user);
        /// <summary>
        /// Find user by id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>returns user</returns>
        Task<User> FindById(string id);
        /// <summary>
        /// Get user roles list
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>returns user roles list</returns>
        Task<IList<string>> GetRoles(User user);
        /// <summary>
        /// Insert new user
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="password">user password</param>
        /// <returns>returns IdentityResult</returns>
        Task<IdentityResult> Insert(User user, string password);
        /// <summary>
        /// Add user to a role
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="role">role</param>
        /// <returns>returns IdentityResult</returns>
        Task<IdentityResult> AddToRole(User user, string role);
        /// <summary>
        /// Update user role
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="newRole">new role</param>
        /// <returns>returns IdentityResult</returns>
        Task<IdentityResult> UpdateRole(User user, string newRole);
        /// <summary>
        /// Find user by email
        /// </summary>
        /// <param name="email">user email</param>
        /// <returns>returns user</returns>
        Task<User> FindByEmail(string email);
        /// <summary>
        /// Generate user password reset token
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>returns user password reset token</returns>
        Task<string> GeneratePasswordResetToken(User user);
        /// <summary>
        /// Reset user password
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="token">user password reset token</param>
        /// <param name="newPassword">user new password</param>
        /// <returns>returns IdentityResult</returns>
        Task<IdentityResult> ResetPassword(User user, string token, string newPassword);
        /// <summary>
        /// Check whether user is allowed for the operation
        /// </summary>
        /// <param name="currentUser">current user</param>
        /// <param name="operationUserId">user who does operation</param>
        /// <param name="alwaysAllowedUserRole">user role which always allowed for the operation</param>
        /// <returns>true if user allowed to does operation, false if not</returns>
        Task<bool> IsUserAllowedForOperation(User currentUser, string operationUserId, string alwaysAllowedUserRole);
        /// <summary>
        /// Count total users
        /// </summary>
        /// <returns>returns users count</returns>
        Task<int> Count();
    }
}
