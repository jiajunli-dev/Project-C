using Data.Abstracts;
using Data.Interfaces;
using Data.Models;

namespace Data.Repositories;

public class MachineRepository : GenericRepository<Machine, int>, IMachineRepository
{
    public MachineRepository(AppDbContext context) : base(context) { }
}
