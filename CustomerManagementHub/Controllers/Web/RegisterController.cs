using CustomerManagementHub.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        public RegisterController(UserManager<UserModel> userManager, IConfiguration config, ILogger<RegisterController> logger)
        {
            _userManager = userManager;
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
        public async Task<IActionResult> PerformRegister([FromForm] RegisterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new UserModel { UserName = model.UserName, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
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
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
