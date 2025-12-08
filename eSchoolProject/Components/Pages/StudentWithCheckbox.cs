using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Components.Pages
{
        public class StudentWithCheckbox
        {
            public StudentViewModel Student { get; set; } = default!;
            public bool IsSelected { get; set; }
        }
}