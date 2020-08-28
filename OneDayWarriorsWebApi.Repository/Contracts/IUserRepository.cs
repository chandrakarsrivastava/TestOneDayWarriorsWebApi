using Microsoft.AspNetCore.Identity;
using OneDayWarriorsWebApi.Entities.Account;
using OneDayWarriorsWebApi.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OneDayWarriorsWebApi.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<ApplicationUser> FindByNameAsync(string userName);
        Task<bool> IsInRoleAsync(ApplicationUser applicationUser, string role);
        Task<IdentityResult> AddToRoleAsync(string userEmail, string role);
        Task<SignInResult> PasswordSignInAsync(string username, string password, bool isPersistent, bool lockoutOnFailure);
        Task<IdentityResult> CreateAsync(ApplicationUser applicationUser, string password);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user);
    }
}
