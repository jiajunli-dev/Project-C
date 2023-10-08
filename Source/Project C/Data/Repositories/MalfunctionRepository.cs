using Data.Exceptions;
using Data.Models;

namespace Data.Repositories
{
    public class MalfunctionRepository
    {
        private readonly AppDbContext _context;
        public MalfunctionRepository(AppDbContext context)
            => _context = context;

        public List<Malfunction> GetAll()
            => _context.Malfunctions.ToList();

        public async Task<Malfunction?> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            if (await _context.Malfunctions.FindAsync(id) is not Malfunction malfunction)
                throw new ModelNotFoundException(nameof(Malfunction));

            return malfunction;
        }

        public async Task<Malfunction> Create(Malfunction malfunction)
        {
            var model = _context.Malfunctions.Add(malfunction);
            await _context.SaveChangesAsync();

            return model.Entity;
        }

        public async Task<Malfunction> Update(Malfunction malfunction)
        {
            if (await _context.Malfunctions.FindAsync(malfunction.MalfunctionId) is null)
                throw new ModelNotFoundException(nameof(Malfunction));

            var result = _context.Malfunctions.Update(malfunction);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            if (await _context.Malfunctions.FindAsync(id) is not Malfunction malfunction)
                throw new ModelNotFoundException(nameof(Malfunction));

            _context.Malfunctions.Remove(malfunction);
            await _context.SaveChangesAsync();
        }
    }
}
