using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using MvcTaskManager.ViewModels;
using OneDayWarriorsWebApi.Data;
using OneDayWarriorsWebApi.Entities.Account;
using OneDayWarriorsWebApi.Logging;
using OneDayWarriorsWebApi.Service.ServiceContracts;
using OneDayWarriorsWebApi.Utilities;
using OneDayWarriorsWebApi.ViewModels;
using System.Threading.Tasks;

namespace OneDayWarriorsWebApi.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IAntiforgery _antiforgery;
        private readonly INLogger _logger;
        private readonly IMessageHelper _messageHelper;

        public AccountController(IUsersService usersService,
            IAntiforgery antiforgery,
            INLogger logger,
            IMessageHelper messageHelper
            )
        {
            this._usersService = usersService;
            this._antiforgery = antiforgery;
            this._logger = logger;
            this._messageHelper = messageHelper;
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginViewModel loginViewModel)
        {
            _logger.LogInformation("testing");
            if (loginViewModel.Username != null || loginViewModel.Password != null)
            {
                var user = await _usersService.Authenticate(new UserLogin
                {
                    Username = loginViewModel.Username,
                    Password = loginViewModel.Password
                });
                if (user == null)
                    return BadRequest(new { message = _messageHelper.GetMessage("UsernamePasswordIncorrect") });

                HttpContext.User = await _usersService.CreateUserPrincipalAsync(user);
                var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
                Response.Headers.Add("Access-Control-Expose-Headers", "XSRF-REQUEST-TOKEN");
                Response.Headers.Add("XSRF-REQUEST-TOKEN", tokens.RequestToken);

                return Ok(user);
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
        }

        [HttpPost]
        [Route("authenticatesocial")]
        public async Task<IActionResult> AuthenticateSocial([FromBody] LoginSocialViewModel signUpViewModel)
        {
            var user = await _usersService.AuthenticateSocial(new LoginSocial { 
             Email = signUpViewModel.Email,
             FirstName = signUpViewModel.FirstName,
             LastName = signUpViewModel.LastName
            });
            if (user == null)
                return BadRequest(new { message = "Invalid Data" });

            HttpContext.User = await _usersService.CreateUserPrincipalAsync(user);
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            Response.Headers.Add("Access-Control-Expose-Headers", "XSRF-REQUEST-TOKEN");
            Response.Headers.Add("XSRF-REQUEST-TOKEN", tokens.RequestToken);

            return Ok(user);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] SignUpViewModel signUpViewModel)
        {
            var user = await _usersService.Register(new UserSignup
            {
                DateOfBirth = signUpViewModel.DateOfBirth,
                Email = signUpViewModel.Email,
                FirstName = signUpViewModel.PersonName.FirstName,
                LastName = signUpViewModel.PersonName.LastName,
                Mobile = signUpViewModel.Mobile,
                Gender = signUpViewModel.Gender,
                Password = signUpViewModel.Password,
                ReceiveNewsLetters = signUpViewModel.ReceiveNewsLetters
            });
            if (user == null)
                return BadRequest(new { message = "Invalid Data" });

            HttpContext.User = await _usersService.CreateUserPrincipalAsync(user);
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            Response.Headers.Add("Access-Control-Expose-Headers", "XSRF-REQUEST-TOKEN");
            Response.Headers.Add("XSRF-REQUEST-TOKEN", tokens.RequestToken);

            return Ok(user);
        }

        [Route("api/getUserByEmail/{Email}")]
        public async Task<IActionResult> GetUserByEmail(string Email)
        {
            var user = await _usersService.GetUserByEmail(Email);
            return Ok(user);
        }

    }
}


