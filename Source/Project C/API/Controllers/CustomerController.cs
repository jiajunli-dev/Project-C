using Data.Exceptions;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerRepository _customerRepository;

    public CustomerController(CustomerRepository repository)
    {
        _customerRepository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerRepository.GetAll();
        if (customers.Count == 0) return NoContent();
        return Ok(customers);
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetById(int customerId)
    {
        try
        {
            var customer = await _customerRepository.GetById(customerId);
            return Ok(customer);
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{customerId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Customer customer)
    {
        if (customer is null)
            throw new ArgumentNullException(nameof(Customer));

        var model = await _customerRepository.Create(customer);
        return Created($"Customer/{model.UserId}", model);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Customer customer)
    {
        if (customer is null)
            return BadRequest("Invalid body content provided");

        try
        {
            return Ok(await _customerRepository.Update(customer));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{customer.UserId}\" was not found");
        }
    }

    [HttpDelete("{customerId}")]
    public async Task<IActionResult> Delete(int customerId)
    {
        if (customerId <= 0)
            return BadRequest("Invalid ID provided");

        try
        {
            await _customerRepository.Delete(customerId);
            return Ok();
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{customerId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
    }
}