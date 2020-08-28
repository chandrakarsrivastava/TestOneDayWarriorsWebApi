using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OneDayWarriorsWebApi.Data;
using OneDayWarriorsWebApi.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneDayWarriorsWebApi.Repository.Managers
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(ApplicationRoleStore applicationRoleStore,
            IEnumerable<IRoleValidator<ApplicationRole>> roleValidators,
            ILookupNormalizer lookupNormalizer,
            IdentityErrorDescriber identityErrorDescriber,
            ILogger<ApplicationRoleManager> logger)
            : base(applicationRoleStore, roleValidators, lookupNormalizer, identityErrorDescriber, logger)
        {

        }
    }
}
