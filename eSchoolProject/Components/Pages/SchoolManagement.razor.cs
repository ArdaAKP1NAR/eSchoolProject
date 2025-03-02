using eSchoolDatabase.ViewModels;
using eSchoolProject.Authorization.Interface;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class SchoolManagement
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Parameter] public long SchoolId { get; set; }
        [Inject] IAuthorizationService AuthorizationService { get; set; } = default!;
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        private SchoolViewModel schoolViewModel = default!;
        private bool IsManagerPopupVisible = false;
        private bool IsTeacherPopupVisible = false;
        private bool IsStudentPopupVisible = false;
        private bool IsClassPopupVisible = false;
        private string ActiveTab = "Managers";
        private void SetActiveTab(string tab)
        {
            ActiveTab = tab;
        }
        private void OpenManagerPopup()
        {
            IsManagerPopupVisible = true;
        }
        private void OpenTeacherPopup()
        {
            IsTeacherPopupVisible = true;
        }
        private void OpenStudentPopup()
        {
            IsStudentPopupVisible = true;
        }
        private void OpenClassPopup()
        {
            IsClassPopupVisible = true;
        }
        protected override async Task OnInitializedAsync()
        {
            await GetSchoolByIdAsync();
            await base.OnInitializedAsync();
        }
        private async Task GetSchoolByIdAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var schoolService = scope.ServiceProvider.GetRequiredService<ISchoolService>();

            schoolViewModel = await schoolService.GetSchoolByIdAsync(SchoolId, cancellationTokenSource.Token);
        }
        private async Task OnSavedAsync()
        {
            await GetSchoolByIdAsync();
        }
    }
}