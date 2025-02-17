using eSchoolDatabase.ViewModels;
using eSchoolProject.Authorization.Interface;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class SchoolOverView
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Parameter] public long SchoolId { get; set; }
        [Inject] IAuthorizationService AuthorizationService { get; set; } = default!;
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        private CancellationTokenSource CancellationTokenSource { get; set; } = new();
        private List<ManagerViewModel> Managers = new();
        private bool IsManagerPopupVisible = false;
        private void OpenManagerPopup()
        {
            IsManagerPopupVisible = true;
        }
        private async Task OnManagerSaved()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IManagerService>();

            Managers = await service.GetManagersBySchoolAsync(SchoolId, CancellationTokenSource.Token);
        }
    }
}