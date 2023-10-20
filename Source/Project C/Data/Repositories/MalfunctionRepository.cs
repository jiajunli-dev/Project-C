using Data.Abstracts;
using Data.Interfaces;
using Data.Models;

namespace Data.Repositories;

public class MalfunctionRepository : GenericRepository<Malfunction, int>, IMalfunctionRepository
{
    public MalfunctionRepository(AppDbContext context) : base(context) { }
}
