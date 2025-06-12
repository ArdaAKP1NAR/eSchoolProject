using Microsoft.EntityFrameworkCore.Storage;

namespace eSchoolProject.Services.IServices
{
    public interface ITransactionService
    {
        IDbContextTransaction BeginTransaction();
    }
}