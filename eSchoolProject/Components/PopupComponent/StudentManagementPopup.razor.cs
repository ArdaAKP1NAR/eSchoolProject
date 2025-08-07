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
        [Parameter] public StudentRequestModel StudentRequestModel { get; set; } = default!;
        [Parameter] public EventCallback PopupClosed { get; set; }
        [Parameter] public EventCallback SaveClicked { get; set; }
        private void OpenedPopup(bool value)
        {
            if (value)
            {
                if (StudentRequestModel == null)
                {
                    StudentRequestModel = new();
                    StudentRequestModel.SchoolId = SchoolId;
                }
            }
            StudentRequestModel.SchoolId = SchoolId;
            _visible = value;
        }
        public void OpenPopup(StudentRequestModel model)
        {
            StudentRequestModel = model;
            IsStudentPopupVisible = true;
        }
        private async Task AddStudentAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IStudentService>();

            await service.AddStudentAsync(StudentRequestModel, cancellationTokenSource.Token);
            Snackbar.Add("Student added succesfuly.", Severity.Success);
           
            await SaveClicked.InvokeAsync(cancellationTokenSource.Token);
           
            IsStudentPopupVisible = false;
        }
        private async Task UpdateStudentAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IStudentService>();
            
            await service.UpdateStudentAsync(StudentRequestModel, cancellationTokenSource.Token);
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