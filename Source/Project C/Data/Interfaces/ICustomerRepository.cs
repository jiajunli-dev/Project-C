using Data.Models;

namespace Data.Interfaces;

public interface ICustomerRepository : IGenericRepository<Customer, int>
{
    Task<Customer?> GetByEmail(string email);
}