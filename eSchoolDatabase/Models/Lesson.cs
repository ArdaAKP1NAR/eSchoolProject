using eSchoolDatabase.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace eSchoolDatabase.Models
{
    public class Lesson : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = default!;
        [MaxLength(100)]
        public string CourseCode { get; set; } = default!;
        public List<Class> ClassList { get; set; } = new();
        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; } = default!;
    }
}
