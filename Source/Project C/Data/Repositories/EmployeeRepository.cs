using Data.Abstracts;
using Data.Interfaces;
using Data.Models;

using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class EmployeeRepository : GenericRepository<Employee, int>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context) { }

    public Task<Employee?> GetByEmail(string email)
      => _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
}
