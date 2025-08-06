using eSchoolDatabase.ViewModels;
using eSchoolProject.Authorization.Interface;
using eSchoolProject.Services;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class ClassDetail
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Parameter] public long ClassId { get; set; }
        [Inject] IAuthorizationService AuthorizationService { get; set; } = default!;
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; init; } = default!;
        [Inject] IConfirmationService ConfirmationService { get; init; } = default!;

        private CancellationTokenSource cancellationTokenSource = new();
        private ClassViewModel? SelectedClass;
        List<StudentWithCheckbox> ClassStudents = new();
        private List<StudentViewModel> UnassignedStudents = new();
        private List<StudentViewModel> SelectedStudents = new();
        private bool IsAddStudentPopupOpen = false;
        private List<ClassViewModel> AvailableClasses = new();
        private long SelectedTargetClassId;
        private void OpenPopup() => IsAddStudentPopupOpen = true;
        private void ClosePopup() => IsAddStudentPopupOpen = false;
        private IEnumerable<StudentWithCheckbox> SelectedClassStudents => ClassStudents.Where(x => x.IsSelected);

        protected override async Task OnInitializedAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();
            SelectedClass = await classService.GetClassByIdAsync(ClassId, cancellationTokenSource.Token);
            
            var studentList = await classService.GetStudentsByClassAsync(ClassId, cancellationTokenSource.Token);

            AvailableClasses = await classService.GetClassesBySchoolAsync(SelectedClass.SchoolId, cancellationTokenSource.Token);
            AvailableClasses = AvailableClasses.Where(c => c.Id != SelectedClass.Id).ToList();

            ClassStudents = studentList.Select(s => new StudentWithCheckbox { Student = s, IsSelected = false }).ToList();

            UnassignedStudents = await classService.GetStudentsWithoutClassBySchoolAsync(SelectedClass.SchoolId, cancellationTokenSource.Token);
        }
       
        private void OnActionWithConfirmationClicked(string message, Func<Task> action)
        {
            ConfirmationService.ShowConfirmSnackbar(message, async () =>
            {
                await action();
            });
        }
        private Task OnSelectedStudentsChanged(IEnumerable<StudentViewModel> students)
        {
            SelectedStudents = students.ToList();
            return Task.CompletedTask;
        }

        private async Task AddStudentToClassAsync()
        {
            if (!SelectedStudents.Any())
            {
                Snackbar.Add("Lütfen en az bir öðrenci seçin.", Severity.Warning);
                return;
            }

            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();

            await classService.AssignStudentsToClassAsync(SelectedStudents, ClassId, cancellationTokenSource.Token);

            SelectedStudents.Clear();

            var studentList = await classService.GetStudentsByClassAsync(ClassId, cancellationTokenSource.Token);
            ClassStudents = studentList.Select(s => new StudentWithCheckbox { Student = s, IsSelected = false }).ToList();

            UnassignedStudents = await classService.GetStudentsWithoutClassBySchoolAsync(SelectedClass!.SchoolId, cancellationTokenSource.Token);

            Snackbar.Add($"{SelectedStudents.Count} öðrenci sýnýfa eklendi.", Severity.Success);
            IsAddStudentPopupOpen = false;

            StateHasChanged();
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

                UnassignedStudents = await classService.GetStudentsWithoutClassBySchoolAsync(SelectedClass!.SchoolId, cancellationTokenSource.Token);

                StateHasChanged();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Hata oluþtu: {ex.Message}", Severity.Error);
            }
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

                var updatedStudents = await classService.GetStudentsByClassAsync(SelectedClass.Id, cancellationTokenSource.Token);
                ClassStudents = updatedStudents.Select(s => new StudentWithCheckbox { Student = s, IsSelected = false }).ToList();

                UnassignedStudents = await classService.GetStudentsWithoutClassBySchoolAsync(SelectedClass!.SchoolId, cancellationTokenSource.Token);
                StateHasChanged();
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

        public class StudentWithCheckbox
        {
            public StudentViewModel Student { get; set; } = default!;
            public bool IsSelected { get; set; }
        }
    }

}