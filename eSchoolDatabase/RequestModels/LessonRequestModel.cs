using eSchoolDatabase.ViewModels;

namespace eSchoolDatabase.RequestModels
{
    public class LessonRequestModel
    {
        public string Name { get; set; } = default!;
        public List<ClassViewModel> ClassList { get; set; } = new();
    }
}
