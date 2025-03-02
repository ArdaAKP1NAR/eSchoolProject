using eSchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels
{
    public class StudentViewModel
    {
        public long Id { get; set; }
        public string IdentityNumber { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int StudentNumber { get; set; }
        public DateTime BirthdayDate { get; set; }
        public AddressViewModel Address { get; set; } = default!;
        public string ParentNumber { get; set; } = default!;
        public List<LessonViewModel>? Lessons { get; set; } = default!;
        public long ClassId { get; set; }
        public List<GradeViewModel>? Grades { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
