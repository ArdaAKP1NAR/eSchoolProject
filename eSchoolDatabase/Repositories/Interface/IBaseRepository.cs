using eSchoolDatabase.Models;

namespace eSchoolDatabase.Repositories.Interface
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsyncById(long Id, CancellationToken cancellationToken = default);
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(long Id, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(long? Id, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    }
}