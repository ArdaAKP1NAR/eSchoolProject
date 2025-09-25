using eSchoolDatabase.Models.Base;
using eSchoolDatabase.Repositories.Interface.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Repositories.Base
{
    public class BaseRepository<T>(eSchoolContext context) : IBaseRepository<T> where T : BaseEntity
    {
        public IQueryable<T> GetAll()
        {
            return context.Set<T>().AsQueryable();
        }
        public async Task<T?> GetByIdAsync(long Id, CancellationToken cancellationToken = default)
        {
            return await GetAll().Where(x => x.Id == Id).SingleOrDefaultAsync(cancellationToken);
        }
        public async Task<T?> GetByIdAsync(long? Id, CancellationToken cancellationToken = default)
        {
            return await GetAll().Where(x => x.Id == Id).SingleOrDefaultAsync(cancellationToken);
        }
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var entry = context.Set<T>().Add(entity);
            await context.SaveChangesAsync(cancellationToken);
            return entry.Entity;
        }
        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsyncById(long Id, CancellationToken cancellationToken = default)
        {
            var entry = await GetByIdAsync(Id);
            await DeleteAsync(entry);
            await context.SaveChangesAsync(cancellationToken);
        }
        public async Task UpdateRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
        {
            context.UpdateRange(entities);
            await context.SaveChangesAsync();
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
