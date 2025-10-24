using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Services;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.PopupComponent
{
    public partial class ClassManagementPopup
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] private ISnackbar Snackbar { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        private bool _visible;
        [Parameter] public long SchoolId { get; set; }
        [Parameter] public EventCallback SaveClicked { get; set; }
        [Parameter] public EventCallback PopupClosed { get; set; }
        [Parameter] public bool IsClassPopupVisible { get => _visible; set { PopupOpened(value); } }
        [Parameter] public long ClassId { get; set; }
        [Parameter] public bool IsAddStudentPopupOpen { get; set; } 
        private List<StudentViewModel> SelectedStudents = new();
        private List<StudentViewModel> UnassignedStudents = new();       
        private ClassRequestModel ClassRequestModel { get; set; } = new();
        private void PopupOpened(bool value)
        {
            if (value)
            {
                ClassRequestModel = new() { SchoolId = SchoolId };
            }
            _visible = value;
        }
        
        private async Task AddClassAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();
          
            await classService.AddClassAsync(ClassRequestModel, SchoolId, cancellationTokenSource.Token);
            Snackbar.Add("Class added successfully", Severity.Success);
          
            await SaveClicked.InvokeAsync();
           
            IsClassPopupVisible = false;
        }
        private async Task ClosePopupAsync()
        {
            IsClassPopupVisible = false;
            IsAddStudentPopupOpen = false;
            await PopupClosed.InvokeAsync();
        }
        public async void OpenAddStudentToClassPopup()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();
            UnassignedStudents = await classService.GetStudentsWithoutClassBySchoolAsync(ClassId, cancellationTokenSource.Token);
            IsAddStudentPopupOpen = true;
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
            Snackbar.Add($"{SelectedStudents.Count} öðrenci sýnýfa eklendi.", Severity.Success);
           
            await SaveClicked.InvokeAsync();

            IsAddStudentPopupOpen = false;
        }
    }
}