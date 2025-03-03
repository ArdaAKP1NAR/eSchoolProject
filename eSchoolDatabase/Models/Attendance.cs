using eSchoolDatabase.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Models
{
    public class Attendance : BaseEntity
    {
        public long? StudentId { get; set; }
        public Student? Student { get; set; } = default!;
        public long LessonId { get; set; }
        public Lesson Lesson { get; set; } = default!;
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
    }

}
