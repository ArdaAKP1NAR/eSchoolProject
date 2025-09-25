using eSchoolDatabase.ViewModels;

namespace eSchoolDatabase.RequestModels
{
    public class LessonRequestModel
    {
        public string Name { get; set; } = default!;
        public string CourseCode { get; set; } = default!;
        public TeacherViewModel? Teacher { get; set; }
        public List<ClassViewModel> ClassList { get; set; } = new();
    }
}
