using eSchoolDatabase.Models.Base;

namespace eSchoolDatabase.Repositories.Interface.Base
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsyncById(long Id, CancellationToken cancellationToken = default);
        IQueryable<T> GetAll(CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(long Id, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(long? Id, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);
    }
}