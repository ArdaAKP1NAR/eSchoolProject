using eSchoolDatabase.Models;
using eSchoolDatabase.RequestModels;
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
        public StudentViewModel()
        {
            Address = new();
        }
        public long Id { get; set; }
        public string IdentityNumber { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int StudentNumber { get; set; }
        public DateTime? BirthdayDate { get; set; }
        public AddressViewModel Address { get; set; } = default!;
        public string ParentNumber { get; set; } = default!;
        public long ClassId { get; set; }
        public List<GradeViewModel> Grades { get; set; } = default!;
        public long SchoolId { get; set; }
        public double? Midterm { get; set; }
        public double? Final { get; set; }
        public double? Oral { get; set; }
        public double? Homework { get; set; }
        public double? Average
        {
            get
            {
                var validGrades = new List<double?> { Midterm, Final, Oral, Homework }.Where(g => g.HasValue).Select(g => g.Value);
                if (!validGrades.Any())
                    return null;
                return Math.Round(validGrades.Average(), 2); // 2 ondalık basamakla yuvarlar
            }
        }
    }
}
