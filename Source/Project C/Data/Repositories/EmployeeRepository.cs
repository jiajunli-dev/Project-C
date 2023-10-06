using Data.Exceptions;

using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class EmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context) => _context = context;

    public async Task<List<Employee>> GetAll() => await _context.Employees.ToListAsync();

    public async Task<Employee> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));
        if (await _context.Employees.FindAsync(id) is not Employee employee)
            throw new ModelNotFoundException(nameof(Employee));

        return employee;
    }

    public async Task<Employee> Create(Employee employee)
    {
        var model = _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return model.Entity;
    }

    public async Task<Employee> Update(Employee employee)
    {
        if (await _context.Employees.FindAsync(employee.UserId) is null)
            throw new ModelNotFoundException(nameof(Employee));

        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task Delete(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(Employee));
        if (await _context.Employees.FindAsync(id) is not Employee employee)
            throw new ModelNotFoundException(nameof(Employee));

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }
}
