using eSchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels
{
    public class ClassViewModel
    {
        public long Id { get; set; }
        public string ClassLevel { get; set; } = default!;
        public string Section { get; set; } = default!;
        public List<TeacherViewModel>? Teachers { get; set; }
        public List<StudentViewModel>? Students { get; set; }
        public long SchoolId { get; set; }
        public string FullClassName => $"{ClassLevel}{Section}";
    }
}
