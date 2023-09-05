namespace Data.Repositories
{
    public class GeneralRepository
    {
        private readonly AppDbContext _context;

        public GeneralRepository(AppDbContext context) => this._context = context;
    }
}
