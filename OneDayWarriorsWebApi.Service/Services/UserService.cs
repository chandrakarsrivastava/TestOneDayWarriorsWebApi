using Microsoft.Extensions.Options;
using OneDayWarriorsWebApi.Entities.Account;
using OneDayWarriorsWebApi.Entities.Identity;
using OneDayWarriorsWebApi.Logging;
using OneDayWarriorsWebApi.Repository.Contracts;
using OneDayWarriorsWebApi.Service.ServiceContracts;
using OneDayWarriorsWebApi.Utilities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OneDayWarriorsWebApi.Service.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        private readonly INLogger _logger;

        public UsersService(IUserRepository userRepository,
            IOptions<AppSettings> appSettings,
            INLogger logger
            )
        {
            this._userRepository = userRepository;
            this._appSettings = appSettings.Value;
            this._logger = logger;
        }

        public async Task<ApplicationUser> Authenticate(UserLogin userLogin)
        {
            var result = await _userRepository.PasswordSignInAsync(userLogin.Username, userLogin.Password, false, false);
            if (result.Succeeded)
            {
                var applicationUser = await _userRepository.FindByNameAsync(userLogin.Username);
                applicationUser.PasswordHash = null;
                if (await this._userRepository.IsInRoleAsync(applicationUser, "Admin")) applicationUser.Role = "Admin";
                else if (await this._userRepository.IsInRoleAsync(applicationUser, "SiteUser")) applicationUser.Role = "SiteUser";

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, applicationUser.Id),
                        new Claim(ClaimTypes.Email, applicationUser.Email),
                        new Claim(ClaimTypes.Role, applicationUser.Role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(8),
                    SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                applicationUser.Token = tokenHandler.WriteToken(token);

                return applicationUser;
            }
            else
            {
                _logger.LogError("Login failed.");
                return null;
            }
        }

        public async Task<ApplicationUser> Register(UserSignup userSignup)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.FirstName = userSignup.FirstName;
            applicationUser.LastName = userSignup.LastName;
            applicationUser.Email = userSignup.Email;
            applicationUser.PhoneNumber = userSignup.Mobile;
            applicationUser.ReceiveNewsLetters = userSignup.ReceiveNewsLetters;
            applicationUser.Gender = userSignup.Gender;
            applicationUser.Role = "SiteUser";
            applicationUser.UserName = userSignup.Email;

            var result = await _userRepository.CreateAsync(applicationUser, userSignup.Password);
            if (result.Succeeded)
            {
                if ((await _userRepository.AddToRoleAsync(userSignup.Email, "SiteUser")).Succeeded)
                {
                    var result2 = await _userRepository.PasswordSignInAsync(userSignup.Email, userSignup.Password, false, false);
                    if (result2.Succeeded)
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
                        var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
                        {
                            Subject = new ClaimsIdentity(new Claim[] {
                            new Claim(ClaimTypes.Name, applicationUser.Id),
                            new Claim(ClaimTypes.Email, applicationUser.Email),
                            new Claim(ClaimTypes.Role, applicationUser.Role)
                            }),
                            Expires = DateTime.UtcNow.AddHours(8),
                            SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials
                                (
                                    new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                                    Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature
                                )
                        };

                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        applicationUser.Token = tokenHandler.WriteToken(token);

                        return applicationUser;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                _logger.LogError("Registration failed.");
                return null;
            }
        }

        public async Task<ApplicationUser> GetUserByEmail(string Email)
        {
            return await _userRepository.FindByEmailAsync(Email);
        }

        public async Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user)
        {
            return await _userRepository.CreateUserPrincipalAsync(user);
        }

        public async Task<ApplicationUser> AuthenticateSocial(LoginSocial loginSocial)
        {
            var applicationUser = await _userRepository.FindByNameAsync(loginSocial.Email);

            if (applicationUser != null)
            {
                // Returning user
                applicationUser.PasswordHash = null;
                if (await this._userRepository.IsInRoleAsync(applicationUser, "Admin")) applicationUser.Role = "Admin";
                else if (await this._userRepository.IsInRoleAsync(applicationUser, "SiteUser")) applicationUser.Role = "SiteUser";

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, applicationUser.Id),
                        new Claim(ClaimTypes.Email, applicationUser.Email),
                        new Claim(ClaimTypes.Role, applicationUser.Role)
                    }),
                    Expires = DateTime.UtcNow.AddHours(8),
                    SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                applicationUser.Token = tokenHandler.WriteToken(token);
                return applicationUser;
            }
            else
            {
                // New user
                applicationUser = new ApplicationUser();
                applicationUser.FirstName = loginSocial.FirstName;
                applicationUser.LastName = loginSocial.LastName;
                applicationUser.Email = loginSocial.Email;
                applicationUser.Role = "SiteUser";
                applicationUser.UserName = loginSocial.Email;

                var result = await _userRepository.CreateAsync(applicationUser, string.Empty);

                if (result.Succeeded)
                {
                    if ((await _userRepository.AddToRoleAsync(loginSocial.Email, "SiteUser")).Succeeded)
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
                        var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
                        {
                            Subject = new ClaimsIdentity(new Claim[] {
                                    new Claim(ClaimTypes.Name, applicationUser.Id),
                                    new Claim(ClaimTypes.Email, applicationUser.Email),
                                    new Claim(ClaimTypes.Role, applicationUser.Role)
                                }),
                            Expires = DateTime.UtcNow.AddHours(8),
                            SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        applicationUser.Token = tokenHandler.WriteToken(token);
                        return applicationUser;
                    }
                }
            }
            return null;
        }
    }
}
