using eSchoolDatabase.RequestModels;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.PopupComponent
{
    public partial class TeacherManagementPopup
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] private ISnackbar Snackbar { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        private bool _visible;
        [Parameter] public bool IsTeacherPopupVisible { get => _visible; set { OpenedPopup(value); } }
        [Parameter] public long SchoolId { get; set; }
        private TeacherRequestModel TeacherRequestModel { get; set; } = new();
        [Parameter] public EventCallback PopupClosed { get; set; }
        [Parameter] public EventCallback SaveClicked { get; set; }
        private void OpenedPopup(bool value)
        {
            if (value)
            {
                if (TeacherRequestModel == null)
                    TeacherRequestModel = new() { SchoolId = SchoolId };
            }
            TeacherRequestModel.SchoolId = SchoolId;
            _visible = value;
        }
        public void OpenPopup(TeacherRequestModel teacherRequestModel)
        {
            TeacherRequestModel = teacherRequestModel;
            IsTeacherPopupVisible = true;
        }
        private async Task AddTeacherAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ITeacherService>();
           
            await service.AddTeacherAsync(TeacherRequestModel, cancellationTokenSource.Token);
            Snackbar.Add("Teacher has been saved succesfuly.", Severity.Success);
           
            await SaveClicked.InvokeAsync(cancellationTokenSource.Token);
            
            IsTeacherPopupVisible = false;
        }
        private async Task UpdateTeacherAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ITeacherService>();

            await service.UpdateTeacherAsync(TeacherRequestModel, cancellationTokenSource.Token);
            Snackbar.Add("Teacher has been updated succesfuly.", Severity.Success);

            await SaveClicked.InvokeAsync(cancellationTokenSource.Token);

            IsTeacherPopupVisible = false;
        }
        private async Task ClosePopupAsync()
        {
            IsTeacherPopupVisible = false;
            await PopupClosed.InvokeAsync(cancellationTokenSource.Token);
        }
    }
}