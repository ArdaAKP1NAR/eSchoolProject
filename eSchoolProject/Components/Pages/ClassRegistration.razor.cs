using eSchoolDatabase.ViewModels;
using eSchoolProject.Authorization.Interface;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class ClassRegistration 
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Parameter] public long SchoolId { get; set; }
        [Inject] IAuthorizationService AuthorizationService { get; set; } = default!;
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        private List<ClassViewModel> classList = new();
        private IEnumerable<long> selectedStudentIds = new List<long>();
        private ClassViewModel? selectedClass;
        private List<StudentViewModel> studentList = new();
        private async Task GetClassBySchoolAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();
            classList = await classService.GetClassesBySchoolAsync(SchoolId, cancellationTokenSource.Token);
        }
        private async Task GetStudentsBySchoolAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var studentService = scope.ServiceProvider.GetRequiredService<IStudentService>();
            studentList = await studentService.GetStudentsBySchoolAsync(SchoolId, cancellationTokenSource.Token);
        }
        protected override async Task OnInitializedAsync()
        {
            await GetClassBySchoolAsync();
            await GetStudentsBySchoolAsync();
            await base.OnInitializedAsync();
        }

        private async Task AddStudentToClassAsync(long classId)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var studentService = scope.ServiceProvider.GetRequiredService<IStudentService>();

            var studentsToRemoveClass = studentList
                .Where(s => !selectedStudentIds.Contains(s.Id))
                .ToList();
            if (studentsToRemoveClass.Any())
            {
                await studentService.RemoveStudentFromClass(classId, studentsToRemoveClass, cancellationTokenSource.Token);
            }
            
            
            if (selectedStudentIds.Any())
            {
                await studentService.AddStudentToClass(classId, selectedStudentIds.ToList(), cancellationTokenSource.Token);
            }
            Snackbar.Add("Student changes were successful", Severity.Success);
        }

        private async Task OnClassChanged(ClassViewModel selected)
        {
            selectedClass = selected;
            if (selectedClass != null)
            {
                using var scope = ServiceScopeFactory.CreateScope();
                var studentService = scope.ServiceProvider.GetRequiredService<IStudentService>();
                var allStudents = await studentService.GetStudentsBySchoolAsync(SchoolId, cancellationTokenSource.Token);

                var studentsInClass = allStudents.Where(s => s.ClassId == selectedClass.Id || s.ClassId == 0).ToList();

                selectedStudentIds = studentsInClass.Where(a => a.ClassId == selectedClass.Id).Select(s => s.Id).ToList();
                studentList = studentsInClass;
            }
        }
        private void CancelSelections()
        {
            selectedClass = null;
            selectedStudentIds.ToList().Clear();  // bu çalýþmýyor
        }
    }
}