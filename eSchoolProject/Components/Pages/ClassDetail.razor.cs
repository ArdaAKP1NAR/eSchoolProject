using eSchoolDatabase.ViewModels;
using eSchoolProject.Authorization.Interface;
using eSchoolProject.Components.PopupComponent;
using eSchoolProject.Exceptions;
using eSchoolProject.Services;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class ClassDetail : IDisposable
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Parameter] public long ClassId { get; set; }
        [Inject] IAuthorizationService AuthorizationService { get; set; } = default!;
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; init; } = default!;
        [Inject] IConfirmationService ConfirmationService { get; init; } = default!;

        [Parameter] public EventCallback StudentAdded { get; set; }
        private CancellationTokenSource cancellationTokenSource = new();
        private ClassViewModel? SelectedClass;

        private MudDataGrid<StudentWithCheckbox> grid;
        List<StudentWithCheckbox> ClassStudents = new();
        private ClassManagementPopup ClassManagementPopup = default!;
        private LessonManagementPopup LessonManagementPopup = default!;
        private List<ClassViewModel> AvailableClasses = new();
        private long SelectedTargetClassId;
        private IEnumerable<StudentWithCheckbox> SelectedClassStudents => ClassStudents.Where(x => x.IsSelected);
        private void LoadStudents()
        {
            grid.ReloadServerData();
        }
        private void OpenPopup()
        {
            ClassManagementPopup.OpenAddStudentToClassPopup();
        }
        private void NavigateToLessonEditPage()
        {
            NavigationManager.NavigateTo($"/lessonsforclass/{ClassId}");
        }
        protected override async Task OnInitializedAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();

            var schoolService = scope.ServiceProvider.GetRequiredService<ISchoolService>();
            SelectedClass = await classService.GetClassByIdAsync(ClassId, cancellationTokenSource.Token);

            var studentList = await classService.GetStudentsByClassAsync(ClassId, cancellationTokenSource.Token);

            AvailableClasses = await schoolService.GetClassesBySchoolAsync(SelectedClass.SchoolId, cancellationTokenSource.Token);
            AvailableClasses = AvailableClasses.Where(c => c.Id != SelectedClass.Id).ToList();

            ClassStudents = studentList.Select(s => new StudentWithCheckbox { Student = s, IsSelected = false }).ToList();

        }
        private void OnActionWithConfirmationClicked(string message, Func<Task> action)
        {
            ConfirmationService.ShowConfirmSnackbar(message, async () =>
            {
                await action();
            });
        }
        private async Task RemoveSelectedStudentsAsync()
        {
            var selectedStudents = ClassStudents
                .Where(s => s.IsSelected)
                .Select(s => s.Student)
                .ToList();

            if (!selectedStudents.Any())
            {
                Snackbar.Add("Please select at least one student.", Severity.Warning);
                return;
            }

            try
            {
                using var scope = ServiceScopeFactory.CreateScope();
                var classService = scope.ServiceProvider.GetRequiredService<IClassService>();

                await classService.RemoveStudentFromClassAsync(selectedStudents, SelectedClass!.Id, cancellationTokenSource.Token);

                Snackbar.Add($"{selectedStudents.Count} öðrenci sýnýftan çýkarýldý.", Severity.Success);

                var updatedStudents = await classService.GetStudentsByClassAsync(SelectedClass.Id, cancellationTokenSource.Token);
                ClassStudents = updatedStudents.Select(s => new StudentWithCheckbox { Student = s, IsSelected = false }).ToList();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                if (ex is ItemNotFoundException)
                {
                    Snackbar.Add(ex.Message);
                }
                Snackbar.Add($"Beklenmedik bir hata oluþtu", Severity.Error);
            }
        }
        private async Task OnStudentSavedAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();

            var studentList = await classService.GetStudentsByClassAsync(ClassId, cancellationTokenSource.Token);
            ClassStudents = studentList.Select(s => new StudentWithCheckbox { Student = s, IsSelected = false }).ToList();
            StateHasChanged();
        }

        private async Task ReassignStudentsToClassAsync()
        {
            var selectedStudents = ClassStudents
                .Where(a => a.IsSelected)
                .Select(s => s.Student)
                .ToList();

            if (!selectedStudents.Any())
            {
                Snackbar.Add("Please select at least one student.", Severity.Warning);
                return;
            }


            if (SelectedTargetClassId == 0)
            {
                Snackbar.Add("Please select a target class.", Severity.Warning);
                return;
            }

            try
            {
                using var scope = ServiceScopeFactory.CreateScope();
                var classService = scope.ServiceProvider.GetRequiredService<IClassService>();

                await classService.ReassignStudentsToClassAsync(selectedStudents, SelectedTargetClassId, cancellationTokenSource.Token);

                Snackbar.Add($"{selectedStudents.Count} öðrenci sýnýfa yeniden atandý.", Severity.Success);

                await OnStudentSavedAsync();
                
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Hata oluþtu: {ex.Message}", Severity.Error);
            }
        }

        private void SelectAllStudents()
        {
            foreach (var student in ClassStudents)
            {
                student.IsSelected = true;
            }
            StateHasChanged();
        }

        private void DeselectAllStudents()
        {
            foreach (var student in ClassStudents)
            {
                student.IsSelected = false;
            }
            StateHasChanged();
        }
        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }

}