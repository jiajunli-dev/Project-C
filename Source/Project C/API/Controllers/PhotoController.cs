using Data.Exceptions;
using Data.Models;
using Data.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class PhotoController : ControllerBase
{
    private readonly ILogger<PhotoController> _logger;
    private readonly PhotoRepository _photoRepository;
    private readonly TicketRepository _ticketRepository;

    public PhotoController(ILogger<PhotoController> logger, PhotoRepository repository, TicketRepository ticketRepository)
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

            return photo is null ? NoContent() : Ok(photo);
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
    public async Task<IActionResult> Create([FromBody] PhotoDto dto)
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
            var photo = Photo.FromDto(dto);
            var model = await _photoRepository.Create(photo);

            return Created($"Photo/{model.PhotoId}", model);
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

        _logger.LogInformation("Updating photo with ID: {photoId}", dto.PhotoId);

        try
        {
            var photo = Photo.FromDto(dto);
            return Ok(await _photoRepository.Update(photo));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"Photo not found with the Id: {dto.PhotoId}");
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict($"Photo with Id: {dto.PhotoId} was updated by another user. Retrieve the latest version and try again.");
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

    [HttpDelete] // DELETE /Photo/1
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
