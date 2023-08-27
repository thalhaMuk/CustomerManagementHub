using CustomerManagementHub.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagementHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CustomerModel> Customers { get; set; }
    }

}
