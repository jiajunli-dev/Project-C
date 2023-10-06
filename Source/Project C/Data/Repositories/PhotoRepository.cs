using Data.Exceptions;
using Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;
public class PhotoRepository
{
    private readonly AppDbContext _context;

    public PhotoRepository(AppDbContext context)
      => _context = context;

    public async Task<List<Photo>> GetAllByTicketId(int ticketId)
      => await _context.TicketPhotos.Where(tp => tp.TicketId == ticketId).Select(tp => tp.Photo).ToListAsync();

    public async Task<Photo> GetById(int photoId)
    {
        if (photoId <= 0)
            throw new ArgumentOutOfRangeException(nameof(photoId));
        if (await _context.Photos.FindAsync(photoId) is not Photo photo)
            throw new ModelNotFoundException(nameof(Photo));

        return photo;
    }

    public async Task<Photo> Create(int ticketId, Photo photo)
    {
        var model = _context.Photos.Add(photo);
        await _context.SaveChangesAsync();
        _context.TicketPhotos.Add(new TicketPhoto { Photo = photo, TicketId = ticketId, PhotoId = model.Entity.PhotoId });
        await _context.SaveChangesAsync();

        return model.Entity;
    }

    public async Task<Photo> Update(Photo photo)
    {
        if (await _context.Tickets.FindAsync(photo.PhotoId) is null)
            throw new ModelNotFoundException(nameof(Photo));

        var model = _context.Photos.Update(photo);
        await _context.SaveChangesAsync();

        return model.Entity;
    }

    public async Task Delete(int photoId)
    {
        var photo = await _context.Photos.FindAsync(photoId) ?? throw new ModelNotFoundException(nameof(Photo));
        if (await _context.TicketPhotos.FirstOrDefaultAsync(tp => tp.PhotoId == photoId) is not TicketPhoto ticketPhoto)
            throw new ModelNotFoundException(nameof(TicketPhoto));

        _context.Photos.Remove(photo);
        _context.TicketPhotos.Remove(ticketPhoto);
        await _context.SaveChangesAsync();
    }
}
