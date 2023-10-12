using AutoMapper;
using CustomerManagementHub.Business.DTOs;
using CustomerManagementHub.Business.Interfaces;
using CustomerManagementHub.DataAccess.Models;
using CustomerManagementHub.Repositories;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace CustomerManagementHub.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IGenericRepository<CustomerModel> _repository;
        private readonly IMapper _mapper;

        public CustomerService(IGenericRepository<CustomerModel> repository, ILogger<CustomerService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task EditCustomer(string customerId, CustomerDTO editModel)
        {
            try
            {
                CustomerModel customer = await _repository.GetByIdAsync(customerId) ?? throw new Exception("Customer not found");

                if (!string.IsNullOrEmpty(editModel.Email))
                {
                    if (!new EmailAddressAttribute().IsValid(editModel.Email))
                    {
                        throw new InvalidDataException("Invalid email");
                    }
                }

                if (!string.IsNullOrEmpty(editModel.Phone))
                {
                    if (!editModel.Phone.All(char.IsDigit) || editModel.Phone.Length != 10)
                    {
                        throw new InvalidDataException("Invalid phone number");
                    }
                }

                _mapper.Map(editModel, customer);

                await _repository.UpdateAsync(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<List<CustomerModel>> GetCustomers()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<List<CustomerModel>> GetCustomersGroupedByZipCode()
        {
            try
            {
                return (await _repository.GetAllAsync()).GroupBy(c => c.Addresszipcode).Select(group => group.First()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                throw new Exception("An error occurred while processing your request");
            }
        }

        public async Task<double> GetDistance(string customerId, double latitude, double longitude)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(customerId) ?? throw new Exception("Customer not found");

                // Calculate distance using latitude and longitude
                double customerLatitude = Convert.ToDouble(customer.Latitude);
                double customerLongitude = Convert.ToDouble(customer.Longitude);
                latitude = Convert.ToDouble(latitude);
                longitude = Convert.ToDouble(longitude);
                double distance = CalculateDistance(customerLatitude, customerLongitude, latitude, longitude);

                return distance;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                throw new Exception("An error occurred while processing your request");
            }
        }

        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
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

        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public async Task<List<CustomerModel>> SearchCustomers(string searchText)
        {
            try
            {
                object searchValue;

                if (int.TryParse(searchText, out int intValue))
                {
                    searchValue = intValue;
                }
                else if (decimal.TryParse(searchText, out decimal decimalValue))
                {
                    searchValue = decimalValue;
                }
                else
                {
                    searchValue = searchText;
                }

                var customers = await _repository.GetAllAsync();
                var filteredCustomers = customers.Where(c =>
                    c.Name.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Phone.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Company.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Addresscity.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Addressstate.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Tags1.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Tags2.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Tags3.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Tags4.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Tags5.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Tags6.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.About.Contains(searchValue.ToString(), StringComparison.OrdinalIgnoreCase) ||
                    c.Number.Equals(searchValue) ||
                    c.Age.Equals(searchValue) ||
                    c.Addressnumber.Equals(searchValue) ||
                    c.Addresszipcode.Equals(searchValue) ||
                    c.Latitude.Equals(searchValue) ||
                    c.Longitude.Equals(searchValue)
                ).ToList();

                return filteredCustomers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request");
                throw new Exception("An error occurred while processing your request");
            }
        }

    }
}
