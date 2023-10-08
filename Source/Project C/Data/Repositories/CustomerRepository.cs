using Data.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class CustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
        => _context = context;

    public Task<List<Customer>> GetAll()
        => _context.Customers.AsNoTracking().ToListAsync();

    public async Task<Customer> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (await _context.Customers.FindAsync(id) is not Customer customer)
            throw new ModelNotFoundException(nameof(Customer));

        return customer;
    }

    public async Task<Customer> Create(Customer customer)
    {
        var model = _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return model.Entity;
    }

    public async Task<Customer> Update(Customer customer)
    {
        if (await _context.Customers.FindAsync(customer.UserId) is null)
            throw new ModelNotFoundException(nameof(Customer));

        var result = _context.Customers.Update(customer);
        await _context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (await _context.Customers.FindAsync(id) is not Customer customer)
            throw new ModelNotFoundException(nameof(Customer));

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}
