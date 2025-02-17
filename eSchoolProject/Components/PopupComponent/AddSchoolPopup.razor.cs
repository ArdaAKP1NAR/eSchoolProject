using eSchoolDatabase.Repositories;
using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;

namespace eSchoolProject.Components.PopupComponent
{
    public partial class AddSchoolPopup
    {
        private bool _visible;
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] private ISnackbar Snackbar { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        [Parameter] public bool IsSchoolPopupVisible { get => _visible; set { visibleChanged(value); } }
        private SchoolRequestModel SchoolRequestModel { get; set; } = new();
        [Parameter] public EventCallback PopupClosed { get; set; }
        [Parameter] public EventCallback SaveClicked { get; set; }
        private void visibleChanged(bool value)
        {
            if (value)
            {
                SchoolRequestModel = SchoolRequestModel.New();
            }
            _visible = value;
        }
        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
        private async Task AddSchoolAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ISchoolService>();

            await service.AddSchoolAsync(SchoolRequestModel, cancellationTokenSource.Token);
            var schools = await service.GetAllSchoolsAsync();

            Snackbar.Add("School saved successfully!", Severity.Success);
            await SaveClicked.InvokeAsync();
            IsSchoolPopupVisible = false;
        }
        private async Task Close()
        {
            IsSchoolPopupVisible = false;
            SchoolRequestModel = new();
            await PopupClosed.InvokeAsync();
        }
    }
}