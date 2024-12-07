using CustomerApi.Interfaces;
using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET: api/customers
    [HttpGet]
    public async Task<ActionResult<List<Customer>>> GetAll()
    {
        var customers = await _customerService.GetAllCustomers();
        return Ok(customers);
    }

    // GET: api/customers/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(int id)
    {
        var customer = await _customerService.GetCustomerById(id);

        if (customer == null)
        {
            return NotFound("Customer not found.");
        }

        return Ok(customer);
    }

    // POST: api/customers
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Customer customer)
    {
        try
        {
            await _customerService.AddCustomer(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }
        catch (ArgumentException)
        {
            return BadRequest("Invalid customer data.");
        }
    }

    // PUT: api/customers/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Customer customer)
    {
        var result = await _customerService.UpdateCustomer(id, customer);

        if (!result)
        {
            return NotFound("Customer not found.");
        }

        return NoContent();
    }

    // DELETE: api/customers/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _customerService.DeleteCustomer(id);

        if (!result)
        {
            return NotFound("Customer not found.");
        }

        return NoContent();
    }
}