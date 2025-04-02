using eSchoolDatabase.ViewModels;
using eSchoolProject.Authorization.Interface;
using eSchoolProject.Components.PopupComponent;
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
        [Inject] ISnackbar Snackbar { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        private SchoolViewModel schoolViewModel = default!;
        private bool IsManagerPopupVisible = false;
        private bool IsTeacherPopupVisible = false;
        private bool IsClassPopupVisible = false;
        private StudentManagementPopup StudentManagementPopup = default!;
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
        private void CreateNewStudent()
        {
            StudentManagementPopup.OpenPopup(new());
        }
        private void OpenStudentPopup(StudentViewModel studentViewModel)
        {
            if (studentViewModel != null)
            {
                StudentManagementPopup.OpenPopup(studentViewModel);
            }
        }
        private void OpenClassPopup()
        {
            IsClassPopupVisible = true;
        }
        private void NavigateToSchool(long Id)
        {
            NavigationManager.NavigateTo($"/RegistrationProcedures/{Id}");
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
            IsManagerPopupVisible = false;
            IsTeacherPopupVisible = false;
            IsClassPopupVisible = false;
            await GetSchoolByIdAsync();
        }
    }
}