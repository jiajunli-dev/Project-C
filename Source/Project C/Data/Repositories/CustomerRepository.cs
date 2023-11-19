using Data.Abstracts;
using Data.Interfaces;
using Data.Models;

namespace Data.Repositories;

public class CustomerRepository : GenericRepository<Customer, string>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }
}
