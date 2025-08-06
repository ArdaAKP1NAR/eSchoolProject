using eSchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels
{
    public class LessonViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public string CourseCode { get; set; } = default!;
        public List<ClassViewModel> ClassList { get; set; } = new();
        public long TeacherId { get; set; }
    }
}
