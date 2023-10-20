using Data.Abstracts;

namespace Data.Interfaces;
public interface IGenericRepository<T, TId> where T : DbModel<TId>
{
    Task<T> Create(T model);
    Task Delete(TId id);
    Task<List<T>> GetAll();
    Task<T> GetById(TId id);
    Task<T> Update(T model);
}