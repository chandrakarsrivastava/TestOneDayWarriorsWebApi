using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using OneDayWarriorsWebApi.Data;
using OneDayWarriorsWebApi.Entities.Account;
using OneDayWarriorsWebApi.Entities.Identity;
using OneDayWarriorsWebApi.Logging;
using OneDayWarriorsWebApi.Repository.Contracts;
using OneDayWarriorsWebApi.Repository.Managers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OneDayWarriorsWebApi.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly ApplicationSignInManager _applicationSignInManager;
        private readonly INLogger _logger;

        public UserRepository(ApplicationUserManager applicationUserManager,
            ApplicationSignInManager applicationSignInManager,
            INLogger logger
            )
        {
            this._applicationUserManager = applicationUserManager;
            this._applicationSignInManager = applicationSignInManager;
            this._logger = logger;
        }
        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await _applicationUserManager.FindByNameAsync(userName);
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser applicationUser, string role)
        {
            return await this._applicationUserManager.IsInRoleAsync(applicationUser, role);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser applicationUser, string password)
        {
            IdentityResult result;
            if (!string.IsNullOrEmpty(password))
            {

                result = await this._applicationUserManager.CreateAsync(applicationUser, password);
            }
            else
            {
                result = await this._applicationUserManager.CreateAsync(applicationUser);
            }

            return result;
        }

        public async Task<IdentityResult> AddToRoleAsync(string userEmail, string role)
        {
            return await _applicationUserManager.AddToRoleAsync(await _applicationUserManager.FindByNameAsync(userEmail), role);
        }

        public async Task<SignInResult> PasswordSignInAsync(string username, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return await _applicationSignInManager.PasswordSignInAsync(username, password, isPersistent, lockoutOnFailure);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _applicationUserManager.FindByEmailAsync(email);
        }

        public async Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user)
        {
            return await _applicationSignInManager.CreateUserPrincipalAsync(user);
        }


    }
}
