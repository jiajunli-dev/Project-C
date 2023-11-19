using Data.Abstracts;
using Data.Interfaces;
using Data.Models;

namespace Data.Repositories;

public class TicketRepository : GenericRepository<Ticket, int>, ITicketRepository
{
    public TicketRepository(AppDbContext context) : base(context) { }

    // TODO implement escalation from Ticket to Malfunction and return Malfunction
    public async Task Escalate(int ticketId)
    {
        throw new NotImplementedException();
    }

    public bool Exists(int ticketId)
      => ticketId > 0 && _context.Tickets.Any(t => t.Id == ticketId);
}
