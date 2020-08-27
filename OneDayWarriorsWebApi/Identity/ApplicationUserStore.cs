//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace OneDayWarriorsWebApi.Identity
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        //test
        public ApplicationUserStore(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {

        }
    }
}


