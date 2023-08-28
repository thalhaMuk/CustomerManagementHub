using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomerManagementHub.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace CustomerManagementHub.Controllers.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(SignInManager<UserModel> signInManager, UserManager<UserModel> userManager, IConfiguration config, ILogger<LoginController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _logger = logger;

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PerformLoginAsync([FromForm] UserLogin userLogin)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userLogin.Username);

                if (user != null && await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false) == Microsoft.AspNetCore.Identity.SignInResult.Success)
                {
                    var token = GenerateToken(user);

                    Response.Headers.Add("Authorization", "Bearer " + token);

                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                        return RedirectToAction("viewall", "Customer");
                    else
                    {
                        return RedirectToAction("index", "Customer");
                    }
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

        private async Task<string> GenerateToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserName)
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

    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
