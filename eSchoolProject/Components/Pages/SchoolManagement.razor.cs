using AutoMapper;
using eSchoolDatabase.Models;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolDatabase.ViewModels.GridViewModels;
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

        private CancellationTokenSource cancellationTokenSource = new();
        private SchoolGridView schoolViewModel = default!;
        private bool IsManagerPopupVisible = false;
        private bool IsTeacherPopupVisible = false;
        private bool IsClassPopupVisible = false;
        private bool IsLessonPopupVisible = false;
        private StudentManagementPopup StudentManagementPopup = default!;
        private MudDataGrid<TeacherGridView> MudDataGridTeachers { get; set; } = default!;
        private MudDataGrid<ManagerGridView> MudDataGridManagers { get; set; } = default!;
        private MudDataGrid<StudentGridView> MudDataGridStudents { get; set; } = default!;
        private MudDataGrid<ClassGridView> MudDataGridClasses { get; set; } = default!;
        private async Task<GridData<TeacherGridView>> LoadTeacher(GridState<TeacherGridView> state)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var schoolService = scope.ServiceProvider.GetRequiredService<ISchoolService>();
            var teachers = await schoolService.GetTeacherForGridBySchoolAsync(SchoolId, cancellationTokenSource.Token);
            return new GridData<TeacherGridView>()
            {
                Items = teachers,
                TotalItems = teachers.Count
            };
        }
        private async Task<GridData<ManagerGridView>> LoadManager(GridState<ManagerGridView> state)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var schoolService = scope.ServiceProvider.GetRequiredService<ISchoolService>();
            var managers = await schoolService.GetManagersForGridBySchoolAsync(SchoolId, cancellationTokenSource.Token);
            return new GridData<ManagerGridView>()
            {
                Items = managers,
                TotalItems = managers.Count
            };
        }
        private async Task<GridData<StudentGridView>> LoadStudent(GridState<StudentGridView> state)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var schoolService = scope.ServiceProvider.GetRequiredService<ISchoolService>();
            var students = await schoolService.GetStudentsForGridBySchoolAsync(SchoolId, cancellationTokenSource.Token);
            return new GridData<StudentGridView>()
            {
                Items = students,
                TotalItems = students.Count
            };
        }
        private async Task<GridData<ClassGridView>> LoadClass(GridState<ClassGridView> state)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var schoolService = scope.ServiceProvider.GetRequiredService<ISchoolService>();
            var classes = await schoolService.GetClassesForGridViewBySchoolAsync(SchoolId, cancellationTokenSource.Token);
            return new GridData<ClassGridView>()
            {
                Items = classes,
                TotalItems = classes.Count
            };
        }
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
        private void OpenStudentPopupFromViewModel(StudentGridView studentGridView)
        {
            if (studentGridView != null)
            {
                var studentRequestModel = Mapper.Map<StudentRequestModel>(studentGridView);
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
        private void NavigateToSchedule(long schoolId)
        {
            NavigationManager.NavigateTo($"/lessonschedulemanagement/{schoolId}");
        }
        protected override async Task OnInitializedAsync()
        {
            await GetSchoolByIdAsync();
            await base.OnInitializedAsync();
        }
        private async Task GetSchoolByIdAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var schoolService = scope.ServiceProvider.GetRequiredService<ISchoolService>();

            schoolViewModel = await schoolService.GetSchoolForGridViewByIdAsync(SchoolId, cancellationTokenSource.Token);
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