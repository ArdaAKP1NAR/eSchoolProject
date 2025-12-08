using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels.GridViewModels
{
    public class LessonGridView
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public string CourseCode { get; set; } = default!;
        public List<ClassViewModel> ClassList { get; set; } = new();
        public TeacherViewModel? Teacher { get; set; }
    }
}
