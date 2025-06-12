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
        private CancellationTokenSource cancellationTokenSource = new();
        private ClassViewModel? SelectedClass;
        private List<StudentViewModel> ClassStudents = new();
        private List<StudentViewModel> UnassignedStudents = new();
        private IEnumerable<long> SelectedStudentIds = new List<long>();
        private bool IsAddStudentDialogOpen = false;

        protected override async Task OnInitializedAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();
            SelectedClass = await classService.GetClassByIdAsync(ClassId, cancellationTokenSource.Token);
            ClassStudents = await classService.GetStudentsByClassAsync(ClassId, cancellationTokenSource.Token);
            UnassignedStudents = await classService.GetStudentsWithoutClassBySchoolAsync(SelectedClass.SchoolId, cancellationTokenSource.Token); // Assuming 0 means unassigned
        }
        private void OpenAddStudentDialog()
        {
            IsAddStudentDialogOpen = true;
        }
        private async Task AddStudentToClassAsync()
        {
            if (!SelectedStudentIds.Any())
            {
                Snackbar.Add("Lütfen en az bir öðrenci seçin.", Severity.Warning);
                return;
            }

            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();
            var selectedStudentsList = SelectedStudentIds.ToList();
            await classService.AssignStudentsToClassAsync(selectedStudentsList, ClassId);

            IsAddStudentDialogOpen = false;

            ClassStudents = await classService.GetStudentsByClassAsync(ClassId, cancellationTokenSource.Token);
            UnassignedStudents = await classService.GetStudentsWithoutClassBySchoolAsync(SelectedClass!.SchoolId, cancellationTokenSource.Token);
            StateHasChanged();
        }
    }
}