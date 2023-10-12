using Microsoft.AspNetCore.Identity;

namespace CustomerManagementHub.Business.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityUser> FindByEmailAsync(string email);
        Task<IdentityUser> FindByNameAsync(string userName);
        Task<IdentityResult> CreateUserAsync(IdentityUser user, string password);
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);
        Task<IdentityResult> AddToRoleAsync(IdentityUser user, string roleName);
        Task<string> GetRolesAsync(IdentityUser user);
    }
}
