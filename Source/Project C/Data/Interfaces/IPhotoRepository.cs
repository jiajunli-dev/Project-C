using Data.Models;

namespace Data.Interfaces;

public interface IPhotoRepository : IGenericRepository<Photo, int>
{
    Task<List<Photo>> GetAllByTicketId(int ticketId);
}
