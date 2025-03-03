using eSchoolDatabase.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace eSchoolDatabase.Models
{
    public class Teacher : BaseUserEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = default!;
        [MaxLength(100)]
        public string PhoneNumber { get; set; } = default!;
        public List<Class> Classes { get; set; } = new();
        public List<Lesson> Lessons { get; set; } = new();
        public School School { get; set; } = default!;
        public long SchoolId { get; set; }
    }
}
