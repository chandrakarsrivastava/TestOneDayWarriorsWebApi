using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneDayWarriorsWebApi.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneDayWarriorsWebApi.Repository.Managers
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser>
    {
        public ApplicationSignInManager(ApplicationUserManager applicationUserManager,
            IHttpContextAccessor httpContextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IOptions<IdentityOptions> options,
            ILogger<ApplicationSignInManager> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<ApplicationUser> app

            )
            : base(applicationUserManager,
                  httpContextAccessor,
                  userClaimsPrincipalFactory,
                  options,
                  logger,
                  schemes,
                  app
                  )
        {

        }
    }
}
