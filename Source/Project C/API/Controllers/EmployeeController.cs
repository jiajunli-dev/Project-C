using System.Security.Claims;

using API.Utility;

using Data.Dtos;
using Data.Exceptions;
using Data.Interfaces;
using Data.Models;
using Data.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IClerkClient _clerk;

    public EmployeeController(ILogger<EmployeeController> logger,
                              IEmployeeRepository repository,
                              IDepartmentRepository departmentRepository,
                              IClerkClient clerk)
    {
        _logger = logger;
        _employeeRepository = repository;
        _departmentRepository = departmentRepository;
        _clerk = clerk;
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.ADMIN}, {Roles.EMPLOYEE}")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all employees");

        try
        {
            var employees = await _employeeRepository.GetAll();

            return employees.Any() ? Ok(employees.Select(m => new EmployeeDto(m)).ToList()) : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching employees");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetById(int employeeId)
    {
        try
        {
            var employee = await _employeeRepository.GetById(employeeId);

            return employee is null ? NoContent() : Ok(new EmployeeDto(employee));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{employeeId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching a employee.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        if (dto is null)
            return BadRequest("Invalid body content provided");

        _logger.LogInformation("Creating employee.");

        string? clerkId = null;
        string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        dto.CreatedBy = userId;
        try
        {
            clerkId = await _clerk.CreateInvitation(dto.Email, dto.Role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the employee in Clerk.");
            return BadRequest($"Employee not created.");
        }

        var dep = await _departmentRepository.Create(new Department { Name = "test", CreatedBy = userId, UpdatedBy = userId, Description = "test" });
        _logger.LogInformation($"Created department {dep.Id}");

        try
        {
            var request = dto.ToModel();
            request.ClerkId = "";
            request.DepartmentId = dep.Id;
            var model = await _employeeRepository.Create(request);

            return Created($"Employee/{model.Id}", new EmployeeDto(model));
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while creating a employee");

            if (clerkId is not null)
                await _clerk.RevokeInvitation(clerkId);
            await _clerk.DeleteUserByEmail(dto.Email);

            return StatusCode(500, "An error occurred while processing your request");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a employee");

            if (clerkId is not null)
                await _clerk.RevokeInvitation(clerkId);
            await _clerk.DeleteUserByEmail(dto.Email);

            return StatusCode(500, "An unexpected error occurred while processing your request");
        }
    }

    [HttpPut]
    [Authorize(Roles = $"{Roles.ADMIN}, {Roles.EMPLOYEE}")]
    public async Task<IActionResult> Update([FromBody] EmployeeDto dto)
    {
        if (dto is null)
            return BadRequest("Invalid body content provided");

        _logger.LogInformation("Updating employee with ID: {employeeId}", dto.Id);

        try
        {
            var model = await _employeeRepository.Update(dto.ToModel());
            return Ok(new EmployeeDto(model));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{dto.Id}\" was not found");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Conflict($"Employee with ID \"{dto.Id}\" was updated by another user. Please refresh and try again.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while updating the employee in the database.");
            return StatusCode(500, "An error occurred while updating the employee in the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the employee in the database.");
            return StatusCode(500, "An error occurred while updating the employee in the database.");
        }
    }

    [HttpDelete("{employeeId}")]
    [Authorize(Roles = $"{Roles.ADMIN}")]
    public async Task<IActionResult> Delete(int employeeId)
    {
        if (employeeId <= 0)
            return BadRequest("Invalid ID provided");

        _logger.LogInformation("Deleting customer with ID: {employeeId}", employeeId);

        var employee = await _employeeRepository.GetById(employeeId);
        if (employee is null)
            return NoContent();

        try
        {
            await _clerk.DeleteUserByEmail(employee.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting the employee from clerk.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }

        try
        {
            await _employeeRepository.Delete(employeeId);

            return NoContent();
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{employeeId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the employee in the database.");
            return StatusCode(500, "An error occurred while deleting the employee in the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting the employee.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }
}
