using Data.Abstracts;
using Data.Interfaces;
using Data.Models;

namespace Data.Repositories;

public class EmployeeRepository : GenericRepository<Employee, string>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context) { }
}
