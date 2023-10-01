using Data.Exceptions;
using Data.Models;
using Data.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
  private readonly TicketRepository _repository;

  public TicketController(TicketRepository repository)
    => _repository = repository;

  [HttpGet] // GET Ticket
  public IActionResult GetAll()
  {
    var tickets = _repository.GetAll();
    if (tickets.Count == 0)
      return NoContent();

    return Ok(tickets);
  }

  [HttpGet("{ticketId}")] // GET Ticket/1
  public async Task<IActionResult> GetById(int ticketId)
  {
    if (ticketId <= 0)
      return BadRequest("Invalid ID provided");

    try
    {
      var ticket = await _repository.GetById(ticketId);
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

  [HttpPost] // POST Ticket
  public async Task<IActionResult> Create([FromBody] Ticket ticket)
  {
    if (ticket is null)
      throw new ArgumentNullException(nameof(ticket));

    var model = await _repository.Create(ticket);
    return Created("Ticket", model);
  }

  [HttpPut] // PUT Ticket
  public async Task<IActionResult> Update([FromBody] Ticket ticket)
  {
    if (ticket is null)
      return BadRequest("Invalid body content provided");

    try
    {
      var model = await _repository.Update(ticket);
      return Ok(model);
    }
    catch (ModelNotFoundException)
    {
      return BadRequest($"A Model with ID \"{ticket.TicketId}\" was not found");
    }
  }

  [HttpDelete("{ticketId}")] // DELETE Ticket/1
  public async Task<IActionResult> Delete(int ticketId)
  {
    if (ticketId <= 0)
      return BadRequest("Invalid ID provided");

    try
    {
      await _repository.Delete(ticketId);
      return Ok();
    }
    catch (ModelNotFoundException)
    {
      return BadRequest($"A Model with ID \"{ticketId}\" was not found");
    }
  }

  [HttpPost("{ticketId}/escalate/{priority}")] // PUT Ticket/1/escalate
  public async Task<IActionResult> Escalate(int ticketId, Priority priority)
  {
    throw new NotImplementedException();
  }
}
