using Data.Exceptions;
using Data.Models;
using Data.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
  private readonly TicketRepository _ticketRepository;
  private readonly PhotoRepository _photoRepository;

  public TicketController(TicketRepository repository, PhotoRepository photoRepository)
  {
    _ticketRepository = repository;
    _photoRepository = photoRepository;
  }

  [HttpGet] // GET Ticket
  public IActionResult GetAll()
  {
    var tickets = _ticketRepository.GetAll();
    if (tickets.Count == 0)
      return NoContent();

    return Ok(tickets);
  }

  [HttpGet("{ticketId}")] // GET Ticket/1
  public async Task<IActionResult> GetById(int ticketId)
  {
    try
    {
      var ticket = await _ticketRepository.GetById(ticketId);
      return Ok(ticket);
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
    if (photos.Count == 0)
      return NoContent();

    return Ok(photos);
  }

  [HttpPost] // POST Ticket
  public async Task<IActionResult> Create([FromBody] Ticket ticket)
  {
    if (ticket is null)
      throw new ArgumentNullException(nameof(ticket));

    var model = await _ticketRepository.Create(ticket);
    return Created($"Ticket/{model.TicketId}", model);
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
  }
}
