using CustomerApi.Models;

namespace CustomerApi.Interfaces;

public interface ICustomerService
{
    Task<List<Customer>> GetAllCustomers();
    Task<Customer?> GetCustomerById(int id);
    Task AddCustomer(Customer customer);
    Task<bool> UpdateCustomer(int id, Customer customer);
    Task<bool> DeleteCustomer(int id);
}