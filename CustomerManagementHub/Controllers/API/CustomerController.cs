using CustomerManagementHub.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using CustomerManagementHub.Controllers.Web;
using CustomerManagementHub.Services;

namespace CustomerManagementHub.Controllers.API
{
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CustomerController : Controller, ICustomerController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ApplicationDbContext context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("Index")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        // Endpoint: api/customer/edit/{customerId}
        [HttpPut("edit/{customerId}")]
        public async Task<IActionResult> EditCustomer(string customerId, [FromBody] CustomerEditModel editModel)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(customerId);

                if (customer == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(editModel.Name))
                {
                    customer.Name = editModel.Name;
                }

                if (!string.IsNullOrEmpty(editModel.Email))
                {
                    if (!new EmailAddressAttribute().IsValid(editModel.Email))
                    {
                        return BadRequest("Error - Invalid email");
                    }
                    customer.Email = editModel.Email;
                }

                if (!string.IsNullOrEmpty(editModel.Phone))
                {
                    if (!editModel.Phone.All(char.IsDigit) || editModel.Phone.Length < 10)
                    {
                        return BadRequest("Error - Invalid phone number");
                    }
                    customer.Phone = editModel.Phone;
                }

                await _context.SaveChangesAsync();
                return Ok("Success - Data updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing customer data.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // Endpoint: api/customer/getdistance/{customerId}
        [HttpGet("getdistance")]
        public IActionResult GetDistance(
            [FromQuery(Name = "customerId")] string customerId,
            [FromQuery(Name = "latitude")] double latitude,
            [FromQuery(Name = "longitude")] double longitude)
        {
            try
            {
                var customer = _context.Customers.Find(customerId);

                if (customer == null)
                {
                    return NotFound();
                }

                // Calculate distance using latitude and longitude
                double customerLatitude = Convert.ToDouble(customer.Latitude);
                double customerLongitude = Convert.ToDouble(customer.Longitude);
                latitude = Convert.ToDouble(latitude);
                longitude = Convert.ToDouble(longitude);
                double distance = CalculateDistance(customerLatitude, customerLongitude, latitude, longitude);

                return Ok(distance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating distance.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Haversine formula 
            double R = 6371; // Earth's radius in kilometers
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;
            return Math.Round(distance, 3);
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        // Endpoint: api/customer/search
        [HttpGet("search")]
        public IActionResult SearchCustomers([FromQuery] string searchText)
        {
            try
            {
                int searchNumber = int.Parse(searchText);
                decimal searchDecimal = decimal.Parse(searchText);
                var customers = _context.Customers.Where(c =>
                    c.Name.Contains(searchText) ||
                    c.Email.Contains(searchText) ||
                    c.Phone.Contains(searchText) ||
                    c.Company.Contains(searchText) ||
                    c.AddressCity.Contains(searchText) ||
                    c.AddressState.Contains(searchText) ||
                    c.Tags1.Contains(searchText) ||
                    c.Tags2.Contains(searchText) ||
                    c.Tags3.Contains(searchText) ||
                    c.Tags4.Contains(searchText) ||
                    c.Tags5.Contains(searchText) ||
                    c.Tags6.Contains(searchText) ||
                    c.About.Contains(searchText) ||
                    c.number.Equals(searchNumber) ||
                    c.Age.Equals(searchNumber) ||
                    c.AddressNumber.Equals(searchNumber) ||
                    c.AddressZipCode.Equals(searchNumber) ||
                    c.Latitude.Equals(searchDecimal) ||
                    c.Longitude.Equals(searchDecimal)
                ).ToList();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating distance.");
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }

        // Endpoint: api/customer/groupedbyzipcode
        [HttpGet("groupedbyzipcode")]
        public IActionResult GetCustomersGroupedByZipCode()
        {
            try
            {
                var customersGrouped = _context.Customers.GroupBy(c => c.AddressZipCode).ToList();
                return Ok(customersGrouped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating distance.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // Endpoint: api/customer/viewall
        [HttpGet("viewall")]
        [Authorize(Roles = "Admin")] 
        public IActionResult ViewAllCustomers()
        {
            try
            {
                var customers = _context.Customers.ToList();
                return View(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating distance.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }

    public class CustomerEditModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
