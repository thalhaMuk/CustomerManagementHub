using CustomerManagementHub.DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace CustomerManagementHub.Business.Services
{
    public interface IUserService
    {
        Task<bool> ValidateUserCredentialsAsync(LoginModel userLogin);
        Task<IdentityUser> GetUserByEmailAsync(string email);
        Task<IdentityResult> RegisterUserAsync(RegisterModel model);
        Task<string> GetUserRoleAsync(IdentityUser user);

    }
}