using eSchoolDatabase;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace eSchoolProject.Services
{
    public class TransactionService(eSchoolContext dbContext) : ITransactionService
    {
        public IDbContextTransaction BeginTransaction() => dbContext.Database.BeginTransaction();

    }
}
