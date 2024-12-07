using CustomerApi.Interfaces;
using CustomerApi.Models;

namespace CustomerApi.Services;

public class CustomerService : ICustomerService
{
    private readonly List<Customer> _customers;

    public CustomerService()
    {
        _customers = new List<Customer>();
    }

    public Task<List<Customer>> GetAllCustomers()
    {
        return Task.FromResult(_customers);
    }

    public Task<Customer?> GetCustomerById(int id)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(customer);
    }

    public Task AddCustomer(Customer customer)
    {
        if (string.IsNullOrEmpty(customer.Name) || string.IsNullOrEmpty(customer.Email))
            throw new ArgumentException("Invalid customer data.");
        
        customer.Id = _customers.Any() ? _customers.Max(c => c.Id) + 1 : 1;

        _customers.Add(customer);
        return Task.CompletedTask;
    }

    public Task<bool> UpdateCustomer(int id, Customer customer)
    {
        var existingCustomer = _customers.FirstOrDefault(c => c.Id == id);
        if (existingCustomer == null) return Task.FromResult(false);

        existingCustomer.Name = customer.Name;
        existingCustomer.Email = customer.Email;
        existingCustomer.Phone = customer.Phone;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteCustomer(int id)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);
        if (customer == null) return Task.FromResult(false);

        _customers.Remove(customer);
        return Task.FromResult(true);
    }
}