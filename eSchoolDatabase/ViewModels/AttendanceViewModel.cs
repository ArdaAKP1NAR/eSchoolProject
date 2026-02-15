using System;

namespace eSchoolDatabase.ViewModels
{
    public class AttendanceViewModel
    {
        public long TeacherId { get; set; } // Added as requested
        public long StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentNumber { get; set; } = string.Empty;
        public long ClassId { get; set; }
        public long LessonId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
        public bool IsRecorded { get; set; } 
    }
}
