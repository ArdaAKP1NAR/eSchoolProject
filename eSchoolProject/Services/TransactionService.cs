using eSchoolDatabase;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace eSchoolProject.Services
{
    public class TransactionService(eSchoolContext dbContext) : ITransactionService
    {
        public async Task ExecuteAsync(Func<Task> operation)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await operation();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
