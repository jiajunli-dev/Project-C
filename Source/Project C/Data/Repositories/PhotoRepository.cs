using Data.Exceptions;
using Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;
public class PhotoRepository
{
    private readonly AppDbContext _context;

    public PhotoRepository(AppDbContext context)
      => _context = context;

    public Task<List<Photo>> GetAllByTicketId(int ticketId)
      => _context.Photos.Where(p => p.TicketId == ticketId).AsNoTracking().ToListAsync();

    public async Task<Photo> GetById(int photoId)
    {
        if (photoId <= 0)
            throw new ArgumentOutOfRangeException(nameof(photoId));
        if (await _context.Photos.FindAsync(photoId) is not Photo photo)
            throw new ModelNotFoundException(nameof(Photo));

        return photo;
    }

    public async Task<Photo> Create(Photo photo)
    {
        var model = _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

        return model.Entity;
    }

    public async Task<Photo> Update(Photo photo)
    {
        if (await _context.Photos.FindAsync(photo.PhotoId) is not Photo existingPhoto)
            throw new ModelNotFoundException(nameof(Photo));

        _context.Entry(existingPhoto).CurrentValues.SetValues(photo);
        await _context.SaveChangesAsync();

        return existingPhoto;
    }

    public async Task Delete(int photoId)
    {
        if (await _context.Photos.FindAsync(photoId) is not Photo photo)
            throw new ModelNotFoundException(nameof(Photo));

        _context.Photos.Remove(photo);
        await _context.SaveChangesAsync();
    }
}
