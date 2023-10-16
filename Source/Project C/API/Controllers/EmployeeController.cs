using Data.Exceptions;
using Data.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly EmployeeRepository _employeeRepository;

    public EmployeeController(ILogger<EmployeeController> logger, EmployeeRepository repository)
    {
        _logger = logger;
        _employeeRepository = repository;
    }

    [HttpGet]
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
    public async Task<IActionResult> GetById(int employeeId)
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

            return Created($"Employee/{model.UserId}", model);
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
    public async Task<IActionResult> Update([FromBody] Employee employee)
    {
        if (employee is null)
            return BadRequest("Invalid body content provided");

        _logger.LogInformation("Updating employee with ID: {employeeId}", employee.UserId);

        try
        {
            return Ok(await _employeeRepository.Update(employee));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{employee.UserId}\" was not found");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Conflict($"Employee with ID \"{employee.UserId}\" was updated by another user. Please refresh and try again.");
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
    public async Task<IActionResult> Delete(int employeeId)
    {
        if (employeeId <= 0)
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
