using Data.Models;

namespace Data.Interfaces;

public interface IEmployeeRepository : IGenericRepository<Employee, int>
{
    Task<Employee?> GetByEmail(string email);
}