using Data.Models;

namespace Data.Interfaces;

public interface ITicketRepository : IGenericRepository<Ticket, int>
{
    Task Escalate(int ticketId);
    bool Exists(int ticketId);
}
