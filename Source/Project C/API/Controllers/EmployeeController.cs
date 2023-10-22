using API.Utility;
using Data.Exceptions;
using Data.Interfaces;
using Data.Models;
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

    public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository repository)
    {
        _logger = logger;
        _employeeRepository = repository;
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.ADMIN}, {Roles.EMPLOYEE}")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all employees");

        try
        {
            var employees = await _employeeRepository.GetAll();

            return employees.Any() ? Ok(employees) : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching employees");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetById(string employeeId)
    {
        try
        {
            var employee = await _employeeRepository.GetById(employeeId);

            return employee is null ? NoContent() : Ok(employee);
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
    public async Task<IActionResult> Create([FromBody] Employee employee)
    {
        if (employee is null)
            return BadRequest("Invalid body content provided");

        _logger.LogInformation("Creating employee.");

        try
        {
            var model = await _employeeRepository.Create(employee);

            return Created($"Employee/{model.Id}", model);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while creating a employee");
            return StatusCode(500, "An error occurred while processing your request");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a employee");
            return StatusCode(500, "An unexpected error occurred while processing your request");
        }
    }

    [HttpPut]
    [Authorize(Roles = $"{Roles.ADMIN}, {Roles.EMPLOYEE}")]
    public async Task<IActionResult> Update([FromBody] Employee employee)
    {
        if (employee is null)
            return BadRequest("Invalid body content provided");

        _logger.LogInformation("Updating employee with ID: {employeeId}", employee.Id);

        try
        {
            return Ok(await _employeeRepository.Update(employee));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{employee.Id}\" was not found");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Conflict($"Employee with ID \"{employee.Id}\" was updated by another user. Please refresh and try again.");
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
    public async Task<IActionResult> Delete(string employeeId)
    {
        if (string.IsNullOrEmpty(employeeId))
            return BadRequest("Invalid ID provided");

        _logger.LogInformation("Deleting customer with ID: {employeeId}", employeeId);

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
