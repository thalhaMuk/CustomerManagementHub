using CustomerManagementHub.Business.DTOs;
using CustomerManagementHub.DataAccess.Models;

namespace CustomerManagementHub.Business.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerModel>> GetCustomers();
        Task EditCustomer(string customerId, CustomerDTO editModel);
        Task<double> GetDistance(string customerId, double latitude, double longitude);
        Task<List<CustomerModel>> SearchCustomers(string searchText);
        Task<List<CustomerModel>> GetCustomersGroupedByZipCode();
    }
}
