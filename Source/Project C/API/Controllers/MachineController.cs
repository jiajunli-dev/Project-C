using API.Utility;

using Data.Dtos;
using Data.Exceptions;
using Data.Interfaces;
using Data.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class MachineController : ControllerBase
{
    private readonly IMachineRepository _machineRepository;
    private readonly ILogger<MachineController> _logger;

    public MachineController(ILogger<MachineController> logger, IMachineRepository machineRepository)
    {
        _logger = logger;
        _machineRepository = machineRepository;
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.ADMIN}, {Roles.EMPLOYEE}")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all machines");

        try
        {
            var machines = await _machineRepository.GetAll();

            return machines.Any() ? Ok(machines) : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching machines");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpGet("{machineId}")]
    public async Task<IActionResult> GetById(int machineId)
    {
        _logger.LogInformation($"Fetching machine with ID {machineId}");

        try
        {
            var machine = await _machineRepository.GetById(machineId);

            return machine is null ? NoContent() : Ok(machine);
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{machineId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while fetching a machine");
            return StatusCode(500, "An unexpected error occurred while processing your request");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMachineDto dto)
    {
        if (dto is null)
            return BadRequest("Invalid body content provided");
        
        _logger.LogInformation($"Creating machine.");

        try
        {
            var model = await _machineRepository.Create(dto.ToModel());

            return Created($"Machine/{model.Id}", model);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"An error occurred while creating the machine in the database.");
            return BadRequest("Machine not created");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while creating a machine");
            return StatusCode(500, "An unexpected error occurred while processing your request");
        }
    }

    [HttpPut]
    [Authorize(Roles = $"{Roles.ADMIN}, {Roles.EMPLOYEE}")]
    public async Task<IActionResult> Update([FromBody] Data.Models.Machine machine)
    {
        if (machine is null)
            return BadRequest("Invalid body content provided");

        _logger.LogInformation($"Updating machine with ID {machine.Id}");

        try
        {
            return Ok(await _machineRepository.Update(machine));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{machine.Id}\" was not found");
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict($"Customer with ID \"{machine.Id}\" was updated by another user. Please refresh and try again.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"An error occurred while updating the machine in the database.");
            return StatusCode(500, "An error occurred while updating the machine in the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while updating a machine");
            return StatusCode(500, "An unexpected error occurred while processing your request");
        }
    }

    [HttpDelete("{machineId}")]
    [Authorize(Roles = $"{Roles.ADMIN}")]
    public async Task<IActionResult> Delete(int machineId)
    {
        if (machineId <= 0)
            return BadRequest("Invalid ID provided");

        _logger.LogInformation($"Deleting machine with ID {machineId}");

        try
        {
            await _machineRepository.Delete(machineId);

            return NoContent();
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{machineId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting the machine from the database.");
            return StatusCode(500, "An error occurred while deleting the machine from the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while deleting a machine");
            return StatusCode(500, "An unexpected error occurred while processing your request");
        }
    }
}
