using System.Security.Claims;

using API.Utility;

using Data.Dtos;
using Data.Exceptions;
using Data.Interfaces;
using Data.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class PhotoController : ControllerBase
{
    private readonly ILogger<PhotoController> _logger;
    private readonly IPhotoRepository _photoRepository;
    private readonly ITicketRepository _ticketRepository;

    public PhotoController(ILogger<PhotoController> logger, IPhotoRepository repository, ITicketRepository ticketRepository)
    {
        _logger = logger;
        _photoRepository = repository;
        _ticketRepository = ticketRepository;
    }

    [HttpGet("{photoId}")] // GET /Photo/1
    public async Task<IActionResult> GetById(int photoId)
    {
        _logger.LogInformation("Fetching photo with ID: {photoId}", photoId);
        
        try
        {
            var photo = await _photoRepository.GetById(photoId);
            return Ok(photo);
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A photo with ID \"{photoId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid Id provided");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching a photo.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpPost] // POST /Photo
    public async Task<IActionResult> Create([FromBody] CreatePhotoDto dto)
    {
        if (dto is null)
            return BadRequest("Invalid body content provided");
        if (dto.TicketId <= 0)
            return BadRequest("Invalid Ticket Id provided");
        if (!_ticketRepository.Exists(dto.TicketId))
            return BadRequest($"Ticket not found with the Id: {dto.TicketId}");

        _logger.LogInformation("Creating photo");

        try
        {
            var model = await _photoRepository.Create(dto.ToPhoto());

            return Created($"Photo/{model.Id}", model);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while creating the photo in the database.");
            return BadRequest("Photo not created.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating the photo.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpPut] // PUT /Photo
    public async Task<IActionResult> Update([FromBody] PhotoDto dto)
    {
        if (dto is null)
            return BadRequest("Invalid body content provided");
        if (dto.TicketId <= 0)
            return BadRequest("Invalid Ticket Id provided");
        if (!_ticketRepository.Exists(dto.TicketId))
            return BadRequest($"Ticket not found with the Id: {dto.TicketId}");
        if (HttpContext.User.IsInRole(Roles.CUSTOMER))
        {
            var accountId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            // check accountId against customer ID
            // if not the same, return 403
        }

        _logger.LogInformation("Updating photo with ID: {photoId}", dto.Id);

        try
        {
            var photo = new Photo(dto);
            var result = await _photoRepository.Update(photo);
            return Ok(new PhotoDto(result));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"Photo not found with the Id: {dto.Id}");
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict($"Photo with Id: {dto.Id} was updated by another user. Retrieve the latest version and try again.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while updating the photo in the database.");
            return StatusCode(500, "An error occurred while updating the photo in the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating the photo.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpDelete("{photoId}")] // DELETE /Photo/1
    public async Task<IActionResult> Delete(int photoId)
    {
        if (photoId <= 0)
            return BadRequest("Invalid ID provided");

        _logger.LogInformation("Deleting photo with ID: {photoId}", photoId);

        try
        {
            await _photoRepository.Delete(photoId);

            return NoContent();
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"Photo not found with the Id: {photoId}");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the photo from the database.");
            return StatusCode(500, "An error occurred while deleting the photo from the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting the photo");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }
}
