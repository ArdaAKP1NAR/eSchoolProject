using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories.Interface.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Repositories.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}
