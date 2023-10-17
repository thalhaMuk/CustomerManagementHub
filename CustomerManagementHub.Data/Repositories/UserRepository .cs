using CustomerManagementHub.Business.Interfaces;
using CustomerManagementHub.DataAccess.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CustomerManagementHub.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserManager<IdentityUser> userManager, ILogger<UserRepository> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IdentityUser> FindByEmailAsync(string email)
        {
            try
            {
                return await _userManager.FindByNameAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while finding user by email");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<IdentityUser> FindByNameAsync(string userName)
        {
            try
            {
                return await _userManager.FindByNameAsync(userName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while finding user");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password)
        {
            try
            {
                return await _userManager.CreateAsync(user, password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            try
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking password for user");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<IdentityResult> AddToRoleAsync(IdentityUser user, string roleName)
        {
            try
            {
                return await _userManager.AddToRoleAsync(user, roleName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user to role");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<string> GetRolesAsync(IdentityUser user)
        {
            try
            {
                bool isAdmin = await _userManager.IsInRoleAsync(user, nameof(Roles.Admin));
                return isAdmin ? nameof(Roles.Admin) : nameof(Roles.User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting roles for user");
                throw new Exception("An error occurred while processing your request");
            }
        }
    }
}
