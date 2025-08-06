using eSchoolDatabase.Models.Base;

namespace eSchoolDatabase.Models
{
    public class Grade : BaseEntity
    {
        public double GradeValue { get; set; }
        public Lesson Lesson { get; set; } = default!;
        public long LessonId { get; set; }
        public Student Student { get; set; } = default!;
        public long StudentId { get; set; }


    }
}
