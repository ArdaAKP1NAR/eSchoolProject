using eSchoolDatabase.Models.Base;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.Repositories.Interface.Base;
using Microsoft.EntityFrameworkCore;

namespace eSchoolDatabase.Repositories.Base
{
    public class UserBaseRepository<T>(eSchoolContext eSchoolContext) : BaseRepository<T>(eSchoolContext), IUserBaseRepository<T> where T : BaseUserEntity
    {
        public async Task<T?> GetUserByIdentityNumberAsync(string IdentityNumber, CancellationToken cancellationToken)
        {
            return await eSchoolContext.Set<T>().FirstOrDefaultAsync(x => x.IdentityNumber == IdentityNumber, cancellationToken);
        }
    }
}
