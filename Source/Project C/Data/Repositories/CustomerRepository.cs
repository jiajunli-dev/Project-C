using Data.Abstracts;
using Data.Interfaces;
using Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerRepository : GenericRepository<Customer, int>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }
    public Task<Customer?> GetByEmail(string email)
      => _context.Customers.FirstOrDefaultAsync(e => e.Email == email);
}
