using Microsoft.AspNetCore.Identity;

namespace CustomerManagementHub.Business.Services
{
    public interface IUserService
    {
        Task<bool> ValidateUserCredentialsAsync(string email, string password);
        Task<IdentityUser> GetUserByEmailAsync(string email);
        Task<IdentityResult> RegisterUserAsync(string userName, string email, string password, string userRole, string adminUsername, string adminPassword);
        Task<string> GetUserRoleAsync(IdentityUser user);

    }
}