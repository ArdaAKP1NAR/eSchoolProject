using eSchoolDatabase.Models.Base;
using eSchoolDatabase.Repositories.Interface.Base;

namespace eSchoolDatabase.Repositories.Interface
{
    public interface IUserBaseRepository<T> : IBaseRepository<T> where T : BaseUserEntity
    {
        Task<T?> GetUserByIdentityNumberAsync(string IdentityNumber, CancellationToken cancellationToken);
    }
}