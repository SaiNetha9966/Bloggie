using Bloggie.Web.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class AppIdentityDbContext : IdentityDbContext<Register>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext>options ) : base(options)

        {
            
        }
    }
}
