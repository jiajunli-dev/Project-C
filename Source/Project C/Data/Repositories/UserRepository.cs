using Data.Abstracts;
using Data.Interfaces;

namespace Data.Models;

public class UserRepository : GenericRepository<User, string>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }
}
