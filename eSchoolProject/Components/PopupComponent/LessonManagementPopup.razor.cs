using AutoMapper;
using eSchoolDatabase.RequestModel;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Components.PopupComponent.Validator;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.PopupComponent
{
    public partial class LessonManagementPopup
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] private ISnackbar Snackbar { get; init; } = default!;
        [Inject] private IMapper mapper { get; init; } = default!;
        private readonly CancellationTokenSource cancellationTokenSource = new();
        [Parameter] public bool IsLessonPopupVisible { get; set; }
        [Parameter] public long SchoolId { get; set; }
        private LessonValidator validationRules = default!;
        private LessonRequestModel LessonRequestModel { get; set; } = new();
        private List<TeacherViewModel> teacherList = new();
        private List<ClassViewModel> classList = new();
        [Parameter] public EventCallback PopupClosed { get; set; }
        [Parameter] public EventCallback SaveClicked { get; set; }

        protected override async Task OnInitializedAsync()
        {

            validationRules = new LessonValidator(ServiceScopeFactory);

            await GetClassBySchoolAsync();
            await base.OnInitializedAsync();
        }
        private async Task GetClassBySchoolAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var schoolService = scope.ServiceProvider.GetRequiredService<ISchoolService>();

            teacherList = await schoolService.GetTeacherBySchoolAsync(SchoolId, cancellationTokenSource.Token);
            classList = await schoolService.GetClassesBySchoolAsync(SchoolId, cancellationTokenSource.Token);
        }

        private async Task AddLessonAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<ILessonService>();

            try
            {
                await service.AddLessonAsync(LessonRequestModel, cancellationTokenSource.Token);

            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
                return;
            }

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