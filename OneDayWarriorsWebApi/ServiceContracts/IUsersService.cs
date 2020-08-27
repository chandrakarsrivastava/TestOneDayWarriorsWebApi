using OneDayWarriorsWebApi.Identity;
using OneDayWarriorsWebApi.ViewModels;
using OneDayWarriorsWebApi.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcTaskManager.ViewModels;

namespace OneDayWarriorsWebApi.ServiceContracts
{
    public interface IUsersService
    {
        Task<ApplicationUser> Authenticate(LoginViewModel loginViewModel);
        Task<ApplicationUser> Register(SignUpViewModel signUpViewModel);
        Task<ApplicationUser> GetUserByEmail(string Email);

    }
}
