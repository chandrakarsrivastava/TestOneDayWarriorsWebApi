using OneDayWarriorsWebApi.Entities.Account;
using OneDayWarriorsWebApi.Entities.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OneDayWarriorsWebApi.Service.ServiceContracts
{
    public interface IUsersService
    {
        Task<ApplicationUser> Authenticate(UserLogin userLogin);
        Task<ApplicationUser> Register(UserSignup userSignup);
        Task<ApplicationUser> GetUserByEmail(string Email);
        Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user);
        Task<ApplicationUser> AuthenticateSocial(LoginSocial loginSocial);

    }
}
