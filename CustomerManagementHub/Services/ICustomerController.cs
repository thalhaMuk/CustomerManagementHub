using CustomerManagementHub.Controllers.API;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagementHub.Services
{
    public interface ICustomerController
    {
        IActionResult Index();
        Task<IActionResult> EditCustomer(string customerId, CustomerEditModel editModel);
        IActionResult GetDistance(string customerId, double latitude, double longitude);
        IActionResult SearchCustomers(string searchText);
        IActionResult GetCustomersGroupedByZipCode();
        IActionResult ViewAllCustomers();
    }
}
