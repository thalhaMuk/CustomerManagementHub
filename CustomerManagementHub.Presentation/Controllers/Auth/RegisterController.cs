using CustomerManagementHub.Business.Services;
using CustomerManagementHub.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagementHub.Presentation.Controllers.Web
{
    [ApiController]
    [Route("api/v{version:apiVersion}/register")]
    [ApiVersion("1.0")]
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(IUserService userService, ILogger<RegisterController> logger)
        {
            _userService = userService;
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
                    var result = await _userService.RegisterUserAsync(model);

                    if (result.Succeeded)
                    {
                        return Redirect("/api/v1.0/Login");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while performing user registration.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
