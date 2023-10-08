using System;
using System.Net.Sockets;
using Data.Exceptions;
using Data.Models;

namespace Data.Repositories
{
    public class DepartmentRepository
    {
        private readonly AppDbContext _context;
        public DepartmentRepository(AppDbContext context) => _context = context;

        public List<Department> GetAll() => _context.Departments.ToList();

        public async Task<Department?> GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }
            if (await _context.Departments.FindAsync(id) is not Department department)
            {
                throw new ModelNotFoundException(nameof(Department));
            }

            return department;
        }

        public async Task<Department> Create(Department department)
        {
            var model = _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return model.Entity;
        }

        public async Task<Department> Update(Department department)
        {
            if (await _context.Departments.FindAsync(department.DepartmentId) is null)
                throw new ModelNotFoundException(nameof(Department));

            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task Delete(int id)
        {
            if (await _context.Departments.FindAsync(id) is not Department department)
                throw new ModelNotFoundException(nameof(Department));

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
