using eSchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels
{
    public class TeacherViewModel
    {
        public long Id { get; set; }
        public string IdentityNumber { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Name { get; set; } = default!;
        [MaxLength(100)]
        public string PhoneNumber { get; set; } = default!;
        public List<ClassViewModel> Classes { get; set; } = new();
        public List<LessonViewModel> Lessons { get; set; } = new();
        public long SchoolId { get; set; }
    }
}
