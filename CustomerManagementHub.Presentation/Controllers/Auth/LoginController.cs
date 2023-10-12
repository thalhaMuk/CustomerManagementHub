using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CustomerManagementHub.Business.Services;
using CustomerManagementHub.DataAccess.Models;

namespace CustomerManagementHub.Presentation.Controllers.Web
{
    [ApiController]
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _config;

        public LoginController(IUserService userService, ILogger<LoginController> logger, IConfiguration config)
        {
            _userService = userService;
            _logger = logger;
            _config = config;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel userLogin)
        {
            try
            {
                var isValidUser = await _userService.ValidateUserCredentialsAsync(userLogin.Username, userLogin.Password);

                if (isValidUser)
                {
                    var user = await _userService.GetUserByEmailAsync(userLogin.Username);
                    var role = await _userService.GetUserRoleAsync(user);
                    var token = GenerateToken(user, role);

                    Response.Headers.Add("Authorization", "Bearer " + token);
                    if(role == "Admin")
                    {
                        return Redirect("https://localhost:7198/api/v1.0/Customer/viewAll");
                    }
                    return Redirect("https://localhost:7198/api/v1.0/Customer");
                }

                _logger.LogWarning($"Login attempt failed for username: {userLogin.Username}");
                return NotFound("Invalid username or password.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while performing login.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                Response.Cookies.Delete(".AspNetCore.Session");
                Response.Cookies.Delete("Identity.Application");
                Response.Cookies.Delete(".AspNetCore.Application");
                return Redirect("https://localhost:7198/Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging out.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        private async Task<string> GenerateToken(IdentityUser user, string role)
          {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }       
    }
}

