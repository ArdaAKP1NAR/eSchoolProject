using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Models
{
    public class Student : BaseEntity
    {
        [MaxLength(100)]
        public string IdentityNumber { get; set; } = default!;
        [MaxLength(100)]
        public string Password { get; set; } = default!;
        [MaxLength(100)]
        public string Name { get; set; } = default!;
        public int StudentNumber { get; set; }
        public DateTime BirthdayDate { get; set; }
        public Address Address { get; set; } = default!;
        public long AddressId { get; set; }
        [MaxLength(100)]
        public string ParentNumber { get; set; } = default!;
        public List<Lesson>? Lessons { get; set; } = default!;
        public Class? Class { get; set; } = default!;
        public long? ClassId { get; set; }
        public List<Grade>? Grades { get; set; } = default!;
        public School School { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
