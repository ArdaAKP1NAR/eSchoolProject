using eSchoolDatabase.RequestModel;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.PopupComponent
{
    public partial class ManagerManagementPopup
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] private ISnackbar Snackbar { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        [Parameter] public bool IsManagerPopupVisible { get; set; }
        [Parameter] public long SchoolId { get; set; }
        private ManagerRequestModel ManagerRequestModel { get; set; } = new();
        [Parameter] public EventCallback PopupClosed { get; set; }
        [Parameter] public EventCallback SaveClicked { get; set; }


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
        private async Task AddManagerAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IManagerService>();

            await service.AddManagerAsync(ManagerRequestModel, SchoolId, cancellationTokenSource.Token);

            Snackbar.Add("Manager added successfully!", Severity.Success);
            await SaveClicked.InvokeAsync();
            IsManagerPopupVisible = false;
        }
        private async Task Close()
        {
            IsManagerPopupVisible = false;
            ManagerRequestModel = new();
            await PopupClosed.InvokeAsync();
        }
    }
}