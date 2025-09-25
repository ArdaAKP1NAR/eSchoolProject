using eSchoolDatabase.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.Models
{
    public class LessonSchedule : BaseEntity
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Class Class { get; set; } = default!;
        public long ClassId { get; set; }
        public Lesson Lesson { get; set; } = default!;
        public long LessonId { get; set; }
        public Teacher Teacher { get; set; } = default!;
        public long TeacherId { get; set; }
    }
}
