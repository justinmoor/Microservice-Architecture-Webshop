using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Sprinters.Authenticatie.DAL
{
    public class AccountsContext : IdentityDbContext
    {
        public AccountsContext(DbContextOptions options) : base(options)
        {

        }
    }
}
