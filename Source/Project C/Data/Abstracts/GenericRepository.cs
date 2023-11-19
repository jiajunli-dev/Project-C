using Data.Exceptions;
using Data.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Data.Abstracts
{
    public abstract class GenericRepository<T, TId> : IGenericRepository<T, TId> where T : DbModel<TId>
    {
        protected readonly AppDbContext _context;

        protected GenericRepository(AppDbContext context)
            => _context = context;

        public virtual Task<List<T>> GetAll()
            => _context.Set<T>().AsNoTracking().ToListAsync();

        public virtual async Task<T> GetById(TId id)
        {
            if (id is int intId && intId <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            if (id is string stringId && string.IsNullOrEmpty(stringId))
                throw new ArgumentNullException(nameof(id));
            if (await _context.Set<T>().FindAsync(id) is not T model)
                throw new ModelNotFoundException(nameof(T));

            return model;
        }

        public virtual async Task<T> Create(T model)
        {
            var result = _context.Set<T>().Add(model);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public virtual async Task<T> Update(T model)
        {
            if (await _context.Set<T>().FindAsync(model.Id) is not T dbModel)
                throw new ModelNotFoundException(nameof(T));

            _context.Entry(dbModel).CurrentValues.SetValues(model);
            await _context.SaveChangesAsync();

            return dbModel;
        }

        public virtual async Task Delete(TId id)
        {
            if (await _context.Set<T>().FindAsync(id) is not T model)
                throw new ModelNotFoundException(nameof(T));

            _context.Set<T>().Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
