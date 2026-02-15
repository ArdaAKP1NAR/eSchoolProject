using System;

namespace eSchoolDatabase.RequestModels
{
    public class AttendanceRequestModel
    {
        public long StudentId { get; set; }
        public long LessonId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }
    }
}
