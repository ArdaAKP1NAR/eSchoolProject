using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;
using eSchoolDatabase.ViewModels.GridViewModels;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class SchoolOverView : IDisposable
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] private NavigationManager NavigationManager { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        private bool IsSchoolPopupVisible = false;
        private MudDataGrid<SchoolGridView> MudDataGrid { get; set; } = default!;

        private async Task<GridData<SchoolGridView>> LoadDataAsync(GridState<SchoolGridView> state)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ISchoolService>();

            var schools = await service.GetAllSchoolsAsync(cancellationTokenSource.Token);
            return new GridData<SchoolGridView>()
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