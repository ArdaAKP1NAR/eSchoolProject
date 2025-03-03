using eSchoolDatabase.RequestModels;
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
        private StudentRequestModel StudentRequestModel { get; set; } = new();
        [Parameter] public EventCallback PopupClosed { get; set; }
        [Parameter] public EventCallback SaveClicked { get; set; }
        private void OpenedPopup(bool value)
        {
            if (value)
            {
                StudentRequestModel = StudentRequestModel.New();
                StudentRequestModel.SchoolId = SchoolId;
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
        private async Task AddStudentAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IStudentService>();

            await service.AddStudentAsync(StudentRequestModel, cancellationTokenSource.Token);
            Snackbar.Add("Student added succesfuly.", Severity.Success);
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