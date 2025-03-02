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
                TeacherRequestModel = new() { SchoolId = SchoolId };
            }
            _visible = value;
        }
        bool isShow;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        void PasswordVisibility()
        {
            if (isShow)
            {
                isShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }
        private async Task AddTeacherAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ITeacherService>();

            await service.AddTeacherAsync(TeacherRequestModel, SchoolId, cancellationTokenSource.Token);
            Snackbar.Add("Teacher has been saved succesfuly.", Severity.Success);
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