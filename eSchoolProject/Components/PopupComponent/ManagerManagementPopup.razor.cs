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
        private bool _visible;
        [Parameter] public bool IsManagerPopupVisible { get => _visible; set { PopupOpened(value); } }
        [Parameter] public long SchoolId { get; set; }
        private ManagerRequestModel ManagerRequestModel { get; set; } = new();
        [Parameter] public EventCallback PopupClosed { get; set; }
        [Parameter] public EventCallback SaveClicked { get; set; }
        private void PopupOpened(bool value)
        {
            if (value)
            {
                ManagerRequestModel = new() { SchoolId = SchoolId };
            }
            _visible = value;
        }
        private async Task AddManagerAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IManagerService>();
            await service.AddManagerAsync(ManagerRequestModel, cancellationTokenSource.Token);
            Snackbar.Add("Manager added successfully!", Severity.Success);
            await SaveClicked.InvokeAsync(cancellationTokenSource.Token);
            IsManagerPopupVisible = false;
        }
        private async Task Close()
        {
            IsManagerPopupVisible = false;
            await PopupClosed.InvokeAsync(cancellationTokenSource.Token);
        }
    }
}