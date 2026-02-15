using eSchoolDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSchoolDatabase.ViewModels
{
    public class LessonScheduleViewModel
    {
        public long Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string StartTimeStr => StartTime.ToString(@"hh\:mm");
        public string EndTimeStr => EndTime.ToString(@"hh\:mm");

        public long ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;

        public long LessonId { get; set; }
        public string LessonName { get; set; } = string.Empty;

        public long TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
    }
}
