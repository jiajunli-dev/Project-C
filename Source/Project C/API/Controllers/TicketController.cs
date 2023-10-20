using API.Utility;

using Data.Dtos;
using Data.Exceptions;
using Data.Interfaces;
using Data.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    private readonly ILogger<TicketController> _logger;
    private readonly ITicketRepository _ticketRepository;
    private readonly IPhotoRepository _photoRepository;

    public TicketController(ILogger<TicketController> logger, ITicketRepository repository, IPhotoRepository photoRepository)
    {
        _logger = logger;
        _ticketRepository = repository;
        _photoRepository = photoRepository;
    }

    [HttpGet] // GET Ticket
    [Authorize(Roles = Roles.ADMIN)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Fetching all tickets");

        try
        {
            var tickets = await _ticketRepository.GetAll();

            return tickets.Any() ? Ok(tickets) : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching tickets");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpGet("{ticketId}")] // GET Ticket/1
    public async Task<IActionResult> GetById(int ticketId)
    {
        _logger.LogInformation("Fetching ticket with ID: {ticketId}", ticketId);

        try
        {
            var ticket = await _ticketRepository.GetById(ticketId);

            return ticket is null ? NoContent() : Ok(ticket);
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A ticket with ID \"{ticketId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching a ticket.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpGet("{ticketId}/photos")] // GET Ticket/1/photos
    public async Task<IActionResult> GetAllByTicketId(int ticketId)
    {
        if (ticketId <= 0)
            return BadRequest("Invalid Id provided");
        if (!_ticketRepository.Exists(ticketId))
            return BadRequest($"Ticket not found with the Id: {ticketId}");

        _logger.LogInformation("Fetching all photos for ticket with ID: {ticketId}", ticketId);

        try
        {
            var photos = await _photoRepository.GetAllByTicketId(ticketId);

            return photos.Any() ? Ok(photos) : NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching photos by ticket ID.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost] // POST Ticket
    public async Task<IActionResult> Create([FromBody] CreateTicketDto dto)
    {
        if (dto is null)
            return BadRequest($"Ticket not provided as JSON body");

        _logger.LogInformation("Creating ticket.");

        try
        {
            var model = await _ticketRepository.Create(dto.ToTicket());

            return Created($"Ticket/{model.Id}", model);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while creating the ticket in the database.");
            return BadRequest($"Ticket not created.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating the ticket");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    [HttpPut] // PUT Ticket
    public async Task<IActionResult> Update([FromBody] Ticket ticket)
    {
        if (ticket is null)
            return BadRequest("Invalid body content provided");

        _logger.LogInformation("Updating ticket with ID: {ticketId}", ticket.Id);

        try
        {
            return Ok(await _ticketRepository.Update(ticket));
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{ticket.Id}\" was not found");
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict($"Ticket with ID {ticket.Id} was updated by another user. Retrieve the latest version and try again.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while updating the ticket in the database.");
            return StatusCode(500, "An error occurred while updating the ticket in the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating the ticket.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }

    // TODO implement Escalate Ticket to malfunction
    [HttpPost("{ticketId}/escalate/")] // PUT Ticket/1/escalate
    public async Task<IActionResult> Escalate(int ticketId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{ticketId}")] // DELETE Ticket/1
    public async Task<IActionResult> Delete(int ticketId)
    {
        if (ticketId <= 0)
            return BadRequest("Invalid ID provided");

        _logger.LogInformation("Deleting ticket with ID: {ticketId}", ticketId);

        try
        {
            await _ticketRepository.Delete(ticketId);

            return NoContent();
        }
        catch (ModelNotFoundException)
        {
            return BadRequest($"A Model with ID \"{ticketId}\" was not found");
        }
        catch (ArgumentOutOfRangeException)
        {
            return BadRequest("Invalid ID provided");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the ticket from the database.");
            return StatusCode(500, "An error occurred while deleting the ticket from the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting the ticket.");
            return StatusCode(500, "An unexpected error occurred while processing your request.");
        }
    }
}
