using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Models
{
    public class Grade : BaseEntity
    {
        public double GradeValue { get; set; }
        public Lesson Lesson { get; set; } = default!;
        public long LessonId { get; set; }
        public long StudentId { get; set; }
        public Student Student { get; set; } = default!;

    }
}
