using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class SchoolOverview : IDisposable
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] private NavigationManager NavigationManager { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        private bool IsSchoolPopupVisible = false;
        private MudDataGrid<SchoolViewModel> MudDataGrid { get; set; } = default!;

        private async Task<GridData<SchoolViewModel>> LoadDataAsync(GridState<SchoolViewModel> state)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ISchoolService>();

            var schools = await service.GetAllSchoolsAsync(cancellationTokenSource.Token);
            return new GridData<SchoolViewModel>()
            {
                Items = schools,
                TotalItems = schools.Count
            };
        }
        private void NavigateToSchool(long Id)
        {
            NavigationManager.NavigateTo($"/SchoolManagement/{Id}");
        }
        private void OpenAddSchoolPopup()
        {
            IsSchoolPopupVisible = true;
        }
        private void NewSchoolAdded()
        {
            IsSchoolPopupVisible = false;
            MudDataGrid.ReloadServerData();
        }
        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }
}