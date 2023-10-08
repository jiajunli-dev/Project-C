using Data.Exceptions;
using Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;
public class TicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context)
        => _context = context;

    public Task<List<Ticket>> GetAll()
        => _context.Tickets.AsNoTracking().ToListAsync();

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
        if (await _context.Tickets.FindAsync(ticket.TicketId) is not Ticket existingTicket)
            throw new ModelNotFoundException(nameof(Ticket));

        _context.Entry(existingTicket).CurrentValues.SetValues(ticket);
        await _context.SaveChangesAsync();

        return existingTicket;
    }

    public async Task Delete(int id)
    {
        if (await _context.Tickets.FindAsync(id) is not Ticket ticket)
            throw new ModelNotFoundException(nameof(Ticket));

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
    }

    // TODO implement escalation from Ticket to Malfunction and return Malfunction
    public async Task Escalate(int ticketId)
    {
        throw new NotImplementedException();
    }

    public bool Exists(int ticketId)
      => ticketId > 0 && _context.Tickets.Any(t => t.TicketId == ticketId);
}
