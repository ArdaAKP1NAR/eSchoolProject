using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.RequestModels
{
    public class LessonScheduleRequestModel
    {
        public long Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public long ClassId { get; set; }
        public long LessonId { get; set; }
        public long TeacherId { get; set; }
    }
}
