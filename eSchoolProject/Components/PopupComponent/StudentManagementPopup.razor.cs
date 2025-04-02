using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.PopupComponent
{
    public partial class StudentManagementPopup
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] private ISnackbar Snackbar { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        private bool _visible;
        [Parameter] public bool IsStudentPopupVisible { get => _visible; set { OpenedPopup(value); } }
        [Parameter] public long SchoolId { get; set; }
        [Parameter] public StudentViewModel StudentViewModel { get; set; } = default!;
        [Parameter] public EventCallback PopupClosed { get; set; }
        [Parameter] public EventCallback SaveClicked { get; set; }
        private void OpenedPopup(bool value)
        {
            if (value)
            {
                if (StudentViewModel == null)
                {
                    StudentViewModel = new();
                    StudentViewModel.SchoolId = SchoolId;
                }
            }
            _visible = value;
        }
        public void OpenPopup(StudentViewModel model)
        {
            StudentViewModel = model;
            IsStudentPopupVisible = true;
        }
        private async Task AddStudentAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IStudentService>();

            await service.AddStudentAsync(StudentViewModel, cancellationTokenSource.Token);
            Snackbar.Add("Student added succesfuly.", Severity.Success);
            await SaveClicked.InvokeAsync(cancellationTokenSource.Token);
            IsStudentPopupVisible = false;
        }
        private async Task UpdateStudentAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IStudentService>();
            await service.UpdateStudentAsync(StudentViewModel, cancellationTokenSource.Token);
            Snackbar.Add("Student updated succesfuly.", Severity.Success);
            await SaveClicked.InvokeAsync(cancellationTokenSource.Token);
            IsStudentPopupVisible = false;
        }
        private async Task ClosePopupAsync()
        {
            IsStudentPopupVisible = false;
            await PopupClosed.InvokeAsync(cancellationTokenSource.Token);
        }
    }
}