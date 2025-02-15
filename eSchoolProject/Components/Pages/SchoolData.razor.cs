using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class SchoolData : IDisposable
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] private NavigationManager NavigationManager { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        private string FilterText = string.Empty;
        private bool IsSchoolPopupVisible = false;
        private MudDataGrid<SchoolViewModel> MudDataGrid { get; set; } = default!;


        private async Task<GridData<SchoolViewModel>> LoadDataAsync(GridState<SchoolViewModel> state)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ISchoolService>();

            var schools = await service.GetAllSchoolsAsync();
            return new GridData<SchoolViewModel>()
            {
                Items = schools,
                TotalItems = schools.Count
            };
  
        }
        private void OpenAddSchoolPopup()
        {
            IsSchoolPopupVisible = true;
        }
        private void NewSchoolAdded()
        {
            MudDataGrid.ReloadServerData();
        }
        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }
}