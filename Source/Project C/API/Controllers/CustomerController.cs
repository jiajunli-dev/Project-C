using API.Utility;

using Data.Dtos;
using Data.Exceptions;
using Data.Interfaces;
using Data.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly ICustomerRepository _customerRepository;
    private readonly IClerkClient _clerk;

    public CustomerController(ILogger<CustomerController> logger,
                              ICustomerRepository repository,
                              IClerkClient clerk)
    {
        _logger = logger;
        _customerRepository = repository;
        _clerk = clerk;
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.ADMIN}, {Roles.EMPLOYEE}")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all customers");

        try
        {
            var customers = await _customerRepository.GetAll();

            return customers.Any() ? Ok(customers) : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching customers");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetById(int customerId)
    {
        _logger.LogInformation($"Fetching customer with ID {customerId}");

        try
        {
            var customer = await _customerRepository.GetById(customerId);

            return customer is null ? NoContent() : Ok(customer);
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{customerId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching a customer.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
    {
        if (dto is null)
            return BadRequest("Invalid body content provided");

        _logger.LogInformation("Creating customer.");

        string? clerkId = null;
        try
        {
            clerkId = await _clerk.CreateInvitation(dto.Email, Roles.CUSTOMER);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the customer in Clerk.");
            return BadRequest($"Customer not created.");
        }

        var model = dto.ToModel();
        var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? string.Empty;
        model.CreatedBy = userId;
        model.UpdatedBy = userId;
        model.ClerkId = "";

        try
        {
            var dbModel = await _customerRepository.Create(model);
            return Created($"Customer/{dbModel.Id}", dbModel);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while creating the customer in the database.");

            if (clerkId is not null)
                await _clerk.RevokeInvitation(clerkId);
            await _clerk.DeleteUserByEmail(dto.Email);

            return BadRequest($"Customer not created.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a customer.");

            if (clerkId is not null)
                await _clerk.RevokeInvitation(clerkId);
            await _clerk.DeleteUserByEmail(dto.Email);

            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpPut]
    [Authorize(Roles = $"{Roles.ADMIN}, {Roles.EMPLOYEE}")]
    public async Task<IActionResult> Update([FromBody] Customer customer)
    {
        if (customer is null)
            return BadRequest("Invalid body content provided");

        _logger.LogInformation("Updating customer with ID: {customerId}", customer.Id);

        try
        {
            return Ok(await _customerRepository.Update(customer));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{customer.Id}\" was not found");
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict($"Customer with ID \"{customer.Id}\" was updated by another user. Please refresh and try again.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while updating the customer in the database.");
            return StatusCode(500, "An error occurred while updating the customer in the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating the customer.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpDelete("{customerId}")]
    [Authorize(Roles = $"{Roles.ADMIN}")]
    public async Task<IActionResult> Delete(int customerId)
    {
        if (customerId <= 0)
            return BadRequest("Invalid ID provided");

        _logger.LogInformation("Deleting customer with ID: {customerId}", customerId);

        var customer = await _customerRepository.GetById(customerId);
        if (customer is null)
            return NoContent();

        try
        {
            await _clerk.DeleteUserByEmail(customer.Email);
            await _clerk.RevokeInvitationByEmail(customer.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting the customer from clerk.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }

        try
        {
            await _customerRepository.Delete(customerId);

            return NoContent();
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{customerId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the customer from the database.");
            return StatusCode(500, "An error occurred while deleting the customer from the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting the customer.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }
}
