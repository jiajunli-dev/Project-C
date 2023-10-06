using Data.Exceptions;
using Data.Models;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeRepository _employeeRepository;

    public EmployeeController(EmployeeRepository repository)
    {
        _employeeRepository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeRepository.GetAll();
        if (employees.Count == 0) return NoContent();
        return Ok(employees);
    }

    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetById(int employeeId)
    {
        try
        {
            var employee = await _employeeRepository.GetById(employeeId);
            return Ok(employee);
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{employeeId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Employee employee)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(Employee));

        var model = await _employeeRepository.Create(employee);
        return Created($"Employee/{model.UserId}", model);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Employee employee)
    {
        if (employee is null)
            return BadRequest("Invalid body content provided");

        try
        {
            return Ok(await _employeeRepository.Update(employee));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{employee.UserId}\" was not found");
        }
    }

    [HttpDelete("{employeeId}")]
    public async Task<IActionResult> Delete(int employeeId)
    {
        if (employeeId <= 0)
            return BadRequest("Invalid ID provided");

        try
        {
            await _employeeRepository.Delete(employeeId);
            return Ok();
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{employeeId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
    }
}
