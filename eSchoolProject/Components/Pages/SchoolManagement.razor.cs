using AutoMapper;
using eSchoolDatabase.Models;
using eSchoolDatabase.RequestModel;
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
    public partial class SchoolManagement : IDisposable
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Parameter] public long SchoolId { get; set; }
        [Inject] IAuthorizationService AuthorizationService { get; set; } = default!;
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; init; } = default!;
        [Inject] IMapper Mapper { get; init; } = default!;

        private CancellationTokenSource cancellationTokenSource = new();
        private SchoolGridView schoolViewModel = default!;
        private bool IsClassPopupVisible = false;
        private bool IsLessonPopupVisible = false;
        private StudentManagementPopup StudentManagementPopup = default!;
        private TeacherManagementPopup TeacherManagementPopup = default!;
        private ManagerManagementPopup ManagerManagementPopup = default!;
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
        private void OpenLessonPopup()
        {
            IsLessonPopupVisible = true;
        }
        private void CreateNewManager()
        {
            ManagerManagementPopup.OpenPopup(new());
        }
        private void OpenManagerPopupFromViewModel(ManagerGridView managerGridView)
        {
            if (managerGridView != null)
            {
                var managerRequestModel = Mapper.Map<ManagerRequestModel>(managerGridView);
                ManagerManagementPopup.OpenPopup(managerRequestModel);
            }
        }
        private void CreateNewTeacher()
        {
            TeacherManagementPopup.OpenPopup(new());
        }
        private void OpenTeacherPopupFromViewModel(TeacherGridView teacherGridView)
        {
            if (teacherGridView != null)
            {
                var teacherRequestModel = Mapper.Map<TeacherRequestModel>(teacherGridView);
                TeacherManagementPopup.OpenPopup(teacherRequestModel);
            }
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
        private async Task NavigateToGradePanel(long teacherId)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ITeacherService>();

            var teacher = await service.GetTeacherByIdAsync(teacherId, cancellationTokenSource.Token);

            if (!teacher.Lessons.Any())
            {
                Snackbar.Add("Bu öðretmenin atanmýþ sýnýfý veya dersi bulunmamaktadýr.", Severity.Warning);
                return;
            }
            NavigationManager.NavigateTo($"/gradepanel/{teacherId}");
        }
        private void NavigateToSchedule(long schoolId)
        {
            NavigationManager.NavigateTo($"/lessonschedulemanagement/{schoolId}");
        }
        private void NavigateToAttendance(long teacherId)
        {
            NavigationManager.NavigateTo($"/attendance/{teacherId}/{SchoolId}");
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
            IsClassPopupVisible = false;
            await GetSchoolByIdAsync();
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }
}