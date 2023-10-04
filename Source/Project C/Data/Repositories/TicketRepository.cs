using Data.Exceptions;
using Data.Models;

namespace Data.Repositories;
public class TicketRepository
{
  private readonly AppDbContext _context;

  public TicketRepository(AppDbContext context) => _context = context;

  public List<Ticket> GetAll() => _context.Tickets.ToList();

  public async Task<Ticket?> GetById(int id)
  {
    if (id <= 0)
      throw new ArgumentOutOfRangeException(nameof(id));
    if (await _context.Tickets.FindAsync(id) is not Ticket ticket)
      throw new ModelNotFoundException(nameof(Ticket));

    return ticket;
  }

  public async Task<Ticket> Create(Ticket ticket)
  {
    var model = _context.Tickets.Add(ticket);
    await _context.SaveChangesAsync();
    return model.Entity;
  }

  public async Task<Ticket> Update(Ticket ticket)
  {
    if (await _context.Tickets.FindAsync(ticket.TicketId) is null)
      throw new ModelNotFoundException(nameof(Ticket));

    _context.Tickets.Update(ticket);
    await _context.SaveChangesAsync();
    return ticket;
  }

  public async Task Delete(int id)
  {
    if (await _context.Tickets.FindAsync(id) is not Ticket ticket)
      throw new ModelNotFoundException(nameof(Ticket));

    _context.Tickets.Remove(ticket);
    await _context.SaveChangesAsync();
  }

  // TODO return created Malfunction
  public async Task Escalate(int ticketId)
  {
    // TODO implement escalation from Ticket to Malfunction
    throw new NotImplementedException();
  }

  public bool Exists(int ticketId)
    => ticketId > 0 && _context.Tickets.Any(t => t.TicketId == ticketId);
}
