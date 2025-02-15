using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Repositories
{
    public class TeacherRepository(eSchoolContext context) : BaseRepository<Teacher>(context), ITeacherRepository
    {
    }
}
