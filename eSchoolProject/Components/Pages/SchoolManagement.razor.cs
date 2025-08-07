using AutoMapper;
using eSchoolDatabase.Models;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Authorization.Interface;
using eSchoolProject.Components.PopupComponent;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class SchoolManagement
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Parameter] public long SchoolId { get; set; }
        [Inject] IAuthorizationService AuthorizationService { get; set; } = default!;
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; init; } = default!;
        [Inject] IMapper Mapper { get; init; } = default!;
        [Inject] ISearchService SearchService { get; init; } = default!;

        private CancellationTokenSource cancellationTokenSource = new();
        private SchoolViewModel schoolViewModel = default!;
        private bool IsManagerPopupVisible = false;
        private bool IsTeacherPopupVisible = false;
        private bool IsClassPopupVisible = false;
        private bool IsLessonPopupVisible = false;
        private StudentManagementPopup StudentManagementPopup = default!;
        private string searchTextManagers = string.Empty;
        private string searchTextTeachers = string.Empty;
        private string searchTextStudents = string.Empty;

        private string ActiveTab = "Managers";
        private void SetActiveTab(string tab)
        {
            ActiveTab = tab;
        }
        private void OpenManagerPopup()
        {
            IsManagerPopupVisible = true;
        }
        private void OpenTeacherPopup()
        {
            IsTeacherPopupVisible = true;
        }
        private void OpenLessonPopup()
        {
            IsLessonPopupVisible = true;
        }
        private void CreateNewStudent()
        {
            StudentManagementPopup.OpenPopup(new());
        }
        private void OpenStudentPopupFromViewModel(StudentViewModel studentViewModel)
        {
            if (studentViewModel != null)
            {
                var studentRequestModel = Mapper.Map<StudentRequestModel>(studentViewModel);
                OpenStudentPopup(studentRequestModel);
            }
        }
        private void OpenStudentPopup(StudentRequestModel studentRequestModel)
        {
            if (studentRequestModel != null)
            {
                StudentManagementPopup.OpenPopup(studentRequestModel);
            }
        }
        private void OpenClassPopup()
        {
            IsClassPopupVisible = true;
        }
        private void NavigateToClassDetail(long Id)
        {
            NavigationManager.NavigateTo($"/class/{Id}");
        }
        private void NavigateToGradePanel(long teacherId)
        {
            NavigationManager.NavigateTo($"/gradepanel/{teacherId}");
        }
        private IEnumerable<TeacherViewModel> FilteredTeachers =>
            SearchService.FilterList(schoolViewModel.Teachers, searchTextTeachers, (teacher, search) =>
                (!string.IsNullOrEmpty(teacher.Name) && teacher.Name.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(teacher.IdentityNumber) && teacher.IdentityNumber.Contains(search, StringComparison.OrdinalIgnoreCase))
            );
        private IEnumerable<StudentViewModel> FilteredStudents =>
             SearchService.FilterList(schoolViewModel.Students, searchTextStudents, (student, search) =>
                 (!string.IsNullOrEmpty(student.Name) && student.Name.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                 student.StudentNumber.ToString().Contains(search) ||
                 (!string.IsNullOrEmpty(student.IdentityNumber) && student.IdentityNumber.Contains(search))
            );
        private IEnumerable<ManagerViewModel> FilteredManagers =>
            SearchService.FilterList(schoolViewModel.Managers, searchTextManagers, (manager, search) =>
                (!string.IsNullOrEmpty(manager.Name) && manager.Name.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(manager.IdentityNumber) && manager.IdentityNumber.Contains(search, StringComparison.OrdinalIgnoreCase))
            );


        protected override async Task OnInitializedAsync()
        {
            await GetSchoolByIdAsync();
            await base.OnInitializedAsync();
        }
        private async Task GetSchoolByIdAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var schoolService = scope.ServiceProvider.GetRequiredService<ISchoolService>();

            schoolViewModel = await schoolService.GetSchoolByIdAsync(SchoolId, cancellationTokenSource.Token);
        }
        private async Task OnSavedAsync()
        {
            IsManagerPopupVisible = false;
            IsTeacherPopupVisible = false;
            IsClassPopupVisible = false;
            await GetSchoolByIdAsync();
        }
    }
}