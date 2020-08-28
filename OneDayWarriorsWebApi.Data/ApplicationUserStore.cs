using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OneDayWarriorsWebApi.Entities.Identity;

namespace OneDayWarriorsWebApi.Data
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        //test
        public ApplicationUserStore(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {

        }
    }
}
