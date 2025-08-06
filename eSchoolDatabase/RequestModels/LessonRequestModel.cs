using eSchoolDatabase.ViewModels;

namespace eSchoolDatabase.RequestModels
{
    public class LessonRequestModel
    {
        public string Name { get; set; } = default!;
        public string CourseCode { get; set; } = default!;
        public long TeacherId { get; set; }
        public List<ClassRequestModel> ClassList { get; set; } = new();
    }
}
