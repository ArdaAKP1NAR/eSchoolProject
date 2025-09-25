using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels.GridViewModels
{
    public class TeacherGridView
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
}
