using Data.Exceptions;
using Data.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserRepository _userRepository;

    public UserController(ILogger<UserController> logger,UserRepository repository)
    {
        _logger = logger;
        _userRepository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all users");
        
        try
        {
            var users = await _userRepository.GetAll();

            return users.Any() ? Ok(users) : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching users");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetById(int userId)
    {
        try
        {
            var user = await _userRepository.GetById(userId);
            return Ok(user);
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{userId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching a user.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        if (user is null)
            return BadRequest("Invalid body content provided");

        _logger.LogInformation("Creating user.");

        try
        {
            var model = await _userRepository.Create(user);
            
            return Created($"User/{model.UserId}", model);
        }
        catch  (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while creating a user");
            return StatusCode(500, "An error occurred while processing your request");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a user.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] User user)
    {
        if (user is null)
            return BadRequest("Invalid body content provided");

        _logger.LogInformation("Updating user with ID: {UserId}", user.UserId);

        try
        {
            return Ok(await _userRepository.Update(user));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{user.UserId}\" was not found");
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict($"User with ID {user.UserId} was updated by another user. Retrieve the latest version and try again.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while updating the user in the database.");
            return StatusCode(500, "An error occurred while updating the user in the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating a user.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(int userId)
    {
        if (userId <= 0)
            return BadRequest("Invalid ID provided");

        _logger.LogInformation("Deleting user with ID: {userId}", userId);

        try
        {
            await _userRepository.Delete(userId);

            return NoContent();
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{userId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the user in the database.");
            return StatusCode(500, "An error occurred while deleting the user in the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting a user.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }
}
