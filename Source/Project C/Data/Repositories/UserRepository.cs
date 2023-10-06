using Data.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task<List<User>> GetAll() => await _context.Users.ToListAsync();

    public async Task<User> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (await _context.Users.FindAsync(id) is not User user)
            throw new ModelNotFoundException(nameof(User));
        
        return user;
    }

    public async Task<User> Create(User user)
    {
        var model = _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return model.Entity;
    }

    public async Task<User> Update(User user)
    {
        if (await _context.Users.FindAsync(user.UserId) is null)
            throw new ModelNotFoundException(nameof(User));

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(User));
        if (await _context.Users.FindAsync(id) is not User user)
            throw new ModelNotFoundException(nameof(User));

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}