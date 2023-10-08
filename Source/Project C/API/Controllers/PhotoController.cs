using Data.Exceptions;
using Data.Models;
using Data.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class PhotoController : ControllerBase
{
    private readonly PhotoRepository _photoRepository;
    private readonly TicketRepository _ticketRepository;

    public PhotoController(PhotoRepository repository, TicketRepository ticketRepository)
    {
        _photoRepository = repository;
        _ticketRepository = ticketRepository;
    }

    [HttpGet("{photoId}")] // GET /Photo/1
    public async Task<IActionResult> GetById(int photoId)
    {
        try
        {
            return Ok(await _photoRepository.GetById(photoId));
        }
        catch (ModelNotFoundException)
        {
            return NoContent();
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid Id provided");
        }
    }

    [HttpPost] // POST /Photo
    public async Task<IActionResult> Create([FromBody] Photo photo)
    {
        if (photo is null)
            return BadRequest("Invalid body content provided");
        if (photo.TicketId <= 0)
            return BadRequest("Invalid Ticket Id provided");
        if (!_ticketRepository.Exists(photo.TicketId))
            return BadRequest($"Ticket not found with the Id: {photo.TicketId}");

        var model = await _photoRepository.Create(photo);
        return Created($"Photo/{model.PhotoId}", model);
    }

    [HttpPut] // PUT /Photo
    public async Task<IActionResult> Update([FromBody] Photo photo)
    {
        if (photo is null)
            return BadRequest("Invalid body content provided");
        if (photo.TicketId <= 0)
            return BadRequest("Invalid Ticket Id provided");
        if (!_ticketRepository.Exists(photo.TicketId))
            return BadRequest($"Ticket not found with the Id: {photo.TicketId}");

        try
        {
            return Ok(await _photoRepository.Update(photo));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"Photo not found with the Id: {photo.PhotoId}");
        }
    }

    [HttpDelete] // DELETE /Photo/1
    public async Task<IActionResult> Delete(int photoId)
    {
        try
        {
            await _photoRepository.Delete(photoId);
            return Ok();
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"Photo not found with the Id: {photoId}");
        }
    }
}
