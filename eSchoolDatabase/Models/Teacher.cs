using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Models
{
    public class Teacher : BaseEntity
    {
        [MaxLength(100)]
        public string IdentityNumber { get; set; } = default!;
        [MaxLength(100)]
        public string Password { get; set; } = default!;
        [MaxLength(100)]
        public string Name { get; set; } = default!;
        [MaxLength(100)]
        public string PhoneNumber { get; set; } = default!;
        public List<Class>? Classes { get; set; } = default!;
        public List<Lesson>? Lessons { get; set; } = default!;
        public School School { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
