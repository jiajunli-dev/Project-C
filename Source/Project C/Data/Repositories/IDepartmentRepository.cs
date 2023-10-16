using Data.Models;

namespace Data.Repositories;
public interface IDepartmentRepository
{
    Task<List<Department>> GetAll();
    Task<Department> Create(Department department);
    Task Delete(int id);
    Task<Department?> GetById(int id);
    Task<Department> Update(Department department);
}