using CustomerManagementHub.Business.DTOs;
using CustomerManagementHub.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagementHub.Controllers.API
{
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/customer")]
    [ApiVersion("1.0")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // Endpoint: customer/edit/{customerId}
        [HttpPut("edit/{customerId}")]
        public async Task<IActionResult> EditCustomer(string customerId, [FromBody] CustomerDTO editModel)
        {
            try
            {
                await _customerService.EditCustomer(customerId, editModel);
                return Ok("Customer updated successfully");
            }
            catch (InvalidDataException ex)
            {
                _logger.LogError(ex, "An error occurred while editing customer data.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing customer data.");
                return BadRequest(ex.Message);
            }
        }

        // Endpoint: customer/getdistance/{customerId}
        [HttpGet("getdistance")]
        public async Task<IActionResult> GetDistance([FromQuery(Name = "customerId")] string customerId,
                                         [FromQuery(Name = "latitude")] double latitude,
                                         [FromQuery(Name = "longitude")] double longitude)
        {
            try
            {
                var result = await _customerService.GetDistance(customerId, latitude, longitude);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating distance.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // Endpoint: customer/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomers([FromQuery] string searchText)
        {
            try
            {
                var result = await _customerService.SearchCustomers(searchText);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for customers.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // Endpoint: customer/groupedbyzipcode
        [HttpGet("groupedbyzipcode")]
        public async Task<IActionResult> GetCustomersGroupedByZipCode()
        {
            try
            {
                var result =  await _customerService.GetCustomersGroupedByZipCode();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting customers grouped by ZIP code.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // Endpoint: customer/viewall
        [HttpGet("viewall")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewAll()
        {
            try
            {
                var listCustomers = await _customerService.GetCustomers();
                return View(listCustomers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all customers.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
