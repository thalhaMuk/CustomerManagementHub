using AutoMapper;
using CustomerManagementHub.Business.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CustomerManagementHub.Business.Interfaces
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IdentityUser> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.FindByEmailAsync(email);
                return _mapper.Map<IdentityUser>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
        {
            try
            {
                var user = await _userRepository.FindByEmailAsync(email);
                if (user == null) return false;
                return await _userRepository.CheckPasswordAsync(user, password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<IdentityResult> RegisterUserAsync(string userName, string email, string password, string userRole, string adminUsername, string adminPassword)
        {
            try
            {
                var user = new IdentityUser { UserName = userName, Email = email };
                var result = await _userRepository.CreateUserAsync(user, password);

                if (result.Succeeded)
                {
                    if (userRole == "Admin")
                    {
                        var admin = await _userRepository.FindByNameAsync(adminUsername);
                        if (admin != null && await _userRepository.CheckPasswordAsync(admin, adminPassword))
                        {
                            await _userRepository.AddToRoleAsync(user, "Admin");
                        }
                        else
                        {
                            return IdentityResult.Failed(new IdentityError { Description = "Failed to create admin." });
                        }
                    }
                    else
                    {
                        await _userRepository.AddToRoleAsync(user, "User");
                    }
                }
                return result;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<string> GetUserRoleAsync(IdentityUser user)
        {
            try
            {
                var roles = await _userRepository.GetRolesAsync(user);
                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                throw new Exception("An error occurred while processing your request");
            }

        }


    }
}

