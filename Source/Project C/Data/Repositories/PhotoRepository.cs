using Data.Abstracts;
using Data.Interfaces;
using Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class PhotoRepository : GenericRepository<Photo, int>, IPhotoRepository
{
    public PhotoRepository(AppDbContext context) : base(context) { }

    public Task<List<Photo>> GetAllByTicketId(int ticketId)
      => _context.Photos.Where(p => p.TicketId == ticketId).AsNoTracking().ToListAsync();
}
