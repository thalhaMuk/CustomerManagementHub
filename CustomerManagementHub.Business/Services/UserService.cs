using AutoMapper;
using CustomerManagementHub.Business.Services;
using CustomerManagementHub.DataAccess.Enums;
using CustomerManagementHub.DataAccess.Models;
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

        public async Task<bool> ValidateUserCredentialsAsync(LoginModel userLogin)
        {
            try
            {
                var user = await _userRepository.FindByEmailAsync(userLogin.Username);
                if (user == null) return false;
                return await _userRepository.CheckPasswordAsync(user, userLogin.Password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            try
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                var result = await _userRepository.CreateUserAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (model.UserRole == nameof(Roles.Admin))
                    {
                        var admin = await _userRepository.FindByNameAsync(model.AdminUsername);
                        if (admin != null && await _userRepository.CheckPasswordAsync(admin, model.AdminPassword))
                        {
                            await _userRepository.AddToRoleAsync(user, nameof(Roles.Admin));
                        }
                        else
                        {
                            return IdentityResult.Failed(new IdentityError { Description = "Failed to create admin." });
                        }
                    }
                    else
                    {
                        await _userRepository.AddToRoleAsync(user, nameof(Roles.User));
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

