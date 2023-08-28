using CustomerManagementHub.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CustomerManagementHub.Controllers.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UserManager<UserModel> _userManager;
        private readonly ILogger<RegisterController> _logger;
        private readonly SignInManager<UserModel> _signInManager;

        public RegisterController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IConfiguration config, ILogger<RegisterController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new UserModel { UserName = model.UserName, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        if (model.UserRole == "Admin")
                        {
                            if(model.AdminPassword == null || model.AdminUsername == null)
                            {
                                return BadRequest("Fill Admin Login Info");
                            }
                            var admin = await _userManager.FindByNameAsync(model.AdminUsername);
                            if (user != null && await _signInManager.CheckPasswordSignInAsync(user, model.AdminPassword, false) == Microsoft.AspNetCore.Identity.SignInResult.Success)
                            {
                                await _userManager.AddToRoleAsync(user, "Admin");
                            }
                            else
                            {
                                BadRequest("Failed to create admin");
                            }
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, "User");
                        }
                        return RedirectToAction("Login", "Login"); 
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while performing user registration.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "User role is required.")]
        public string UserRole { get; set; }

        // Admin-specific fields
        public string? AdminUsername { get; set; }
        public string? AdminPassword { get; set; }
    }
}
