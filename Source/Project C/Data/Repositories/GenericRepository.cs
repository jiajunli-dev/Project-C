using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public abstract class GenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public virtual Task<List<T>> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToListAsync();
        }
    }
}
