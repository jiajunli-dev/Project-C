using Data.Models;

namespace Data.Interfaces;

public interface IEmployeeRepository : IGenericRepository<Employee, string>
{
}