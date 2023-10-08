using Data.Exceptions;
using Data.Models;
using Data.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Serilog;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    private readonly ILogger<TicketController> _logger;
    private readonly TicketRepository _ticketRepository;
    private readonly PhotoRepository _photoRepository;

    public TicketController(ILogger<TicketController> logger, TicketRepository repository, PhotoRepository photoRepository)
    {
        _logger = logger;
        _ticketRepository = repository;
        _photoRepository = photoRepository;
    }

    [HttpGet] // GET Ticket
    public async Task<IActionResult> GetAll()
    {
        var tickets = await _ticketRepository.GetAll();

        return tickets.Any() ? Ok(tickets) : NoContent();
    }

    [HttpGet("{ticketId}")] // GET Ticket/1
    public async Task<IActionResult> GetById(int ticketId)
    {
        try
        {
            return Ok(await _ticketRepository.GetById(ticketId));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{ticketId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
    }

    [HttpGet("{ticketId}/photos")] // GET Ticket/1/photos
    public async Task<IActionResult> GetAllByTicketId(int ticketId)
    {
        if (ticketId <= 0)
            return BadRequest("Invalid Id provided");
        if (!_ticketRepository.Exists(ticketId))
            return BadRequest($"Ticket not found with the Id: {ticketId}");

        var photos = await _photoRepository.GetAllByTicketId(ticketId);

        return photos.Any() ? Ok(photos) : NoContent();
    }

    [HttpPost] // POST Ticket
    public async Task<IActionResult> Create([FromBody] Ticket ticket)
    {
        _logger.LogInformation($"Creating Ticket");

        if (ticket is null)
        {
            _logger.LogWarning("Ticket not provided as JSON body");
            return BadRequest($"Ticket not provided as JSON body");
        }

        try
        {
            var model = await _ticketRepository.Create(ticket);
            return Created($"Ticket/{model.TicketId}", model);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError("An update concurrency occured while trying to create ticket", ex);
            return Conflict(await _ticketRepository.GetById(ticket.TicketId));
        }
        catch (DbUpdateException)
        {
            return BadRequest($"Ticket not created");
        }
    }

    [HttpPut] // PUT Ticket
    public async Task<IActionResult> Update([FromBody] Ticket ticket)
    {
        if (ticket is null)
            return BadRequest("Invalid body content provided");

        try
        {
            return Ok(await _ticketRepository.Update(ticket));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{ticket.TicketId}\" was not found");
        }
    }

    [HttpPost("{ticketId}/escalate/")] // PUT Ticket/1/escalate
    public async Task<IActionResult> Escalate(int ticketId)
    {
        // TODO implement Escalate Ticket to malfunction
        throw new NotImplementedException();
    }

    [HttpDelete("{ticketId}")] // DELETE Ticket/1
    public async Task<IActionResult> Delete(int ticketId)
    {
        if (ticketId <= 0)
            return BadRequest("Invalid ID provided");

        try
        {
            await _ticketRepository.Delete(ticketId);
            return Ok();
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{ticketId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
    }
}
