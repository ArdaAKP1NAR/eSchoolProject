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
        private List<StudentViewModel> SelectedStudents = new();
        private bool IsAddStudentPopupOpen = false;

        protected override async Task OnInitializedAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();
            SelectedClass = await classService.GetClassByIdAsync(ClassId, cancellationTokenSource.Token);
            ClassStudents = await classService.GetStudentsByClassAsync(ClassId, cancellationTokenSource.Token);
            UnassignedStudents = await classService.GetStudentsWithoutClassBySchoolAsync(SelectedClass.SchoolId, cancellationTokenSource.Token); // Assuming 0 means unassigned
        }
        private void OpenPopup()
        {
            IsAddStudentPopupOpen = true;
        }
        private void ClosePopup()
        {
            IsAddStudentPopupOpen = false;
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

            await classService.AssignStudentsToClassAsync(SelectedStudents, ClassId);

            IsAddStudentPopupOpen = false;

            ClassStudents = await classService.GetStudentsByClassAsync(ClassId, cancellationTokenSource.Token);
            UnassignedStudents = await classService.GetStudentsWithoutClassBySchoolAsync(SelectedClass!.SchoolId, cancellationTokenSource.Token);
            StateHasChanged();
        }
    }
}