using eSchoolDatabase.RequestModel;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.PopupComponent
{
    public partial class LessonManagementPopup
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] private ISnackbar Snackbar { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        private bool _visible;
        [Parameter] public bool IsLessonPopupVisible { get; set; }
        [Parameter] public long SchoolId { get; set; }
        private LessonRequestModel LessonRequestModel { get; set; } = new();
        private List<ClassViewModel> classList = new();
        private IEnumerable<long> selectedClassIds = new List<long>();
        [Parameter] public EventCallback PopupClosed { get; set; }
        [Parameter] public EventCallback SaveClicked { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetClassBySchoolAsync();
            await base.OnInitializedAsync();
        }
        private async Task GetClassBySchoolAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();
            classList = await classService.GetClassesBySchoolAsync(SchoolId, cancellationTokenSource.Token);
        }
        private async Task AddLessonAsync()
        {
            var addLessonToClassEntitites = classList
                .Where(c => selectedClassIds.Contains(c.Id))
                .ToList();

            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ILessonService>();
            await service.AddLessonAsync(LessonRequestModel, addLessonToClassEntitites, cancellationTokenSource.Token);
            Snackbar.Add("Lesson added successfully!", Severity.Success);
            await SaveClicked.InvokeAsync(cancellationTokenSource.Token);
            IsLessonPopupVisible = false;
        }
        private async Task Close()
        {
            IsLessonPopupVisible = false;
            await PopupClosed.InvokeAsync(cancellationTokenSource.Token);
        }
    }
}