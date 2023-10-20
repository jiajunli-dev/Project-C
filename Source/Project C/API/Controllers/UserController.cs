using Data.Abstracts;
using Data.Exceptions;
using Data.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository repository)
    {
        _userRepository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepository.GetAll();
        if (users.Count == 0) return NoContent();
        return Ok(users);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetById(string userId)
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
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        if (user is null)
            return BadRequest("Invalid body content provided");

        var model = await _userRepository.Create(user);
        return Created($"User/{model.Id}", model);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] User user)
    {
        if (user is null)
            return BadRequest("Invalid body content provided");

        try
        {
            return Ok(await _userRepository.Update(user));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{user.Id}\" was not found");
        }
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return BadRequest("Invalid ID provided");

        try
        {
            await _userRepository.Delete(userId);
            return Ok();
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{userId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
    }
}
