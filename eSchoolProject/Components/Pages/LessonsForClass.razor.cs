using AutoMapper;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolDatabase.ViewModels.GridViewModels;
using eSchoolProject.Authorization.Interface;
using eSchoolProject.Components.PopupComponent;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class LessonsForClass
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Parameter] public long ClassId { get; set; }
        [Inject] IAuthorizationService AuthorizationService { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; init; } = default!;
        [Inject] IMapper Mapper { get; init; } = default!;
        private CancellationTokenSource cancellationTokenSource = new();
        private MudDataGrid<LessonGridView> MudDataGrid { get; set; } = default!;
        private LessonManagementPopup LessonManagementPopup = default!;
        private long SchoolId;
        protected override async Task OnInitializedAsync()
        {
            await GetSchoolId();
            await base.OnInitializedAsync();
        }
        private async Task GetSchoolId()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var classService = scope.ServiceProvider.GetRequiredService<IClassService>();
            var classModel = await classService.GetClassByIdAsync(ClassId, cancellationTokenSource.Token);
            SchoolId = classModel.SchoolId;
        }
        private async Task<GridData<LessonGridView>> LoadDataAsync(GridState<LessonGridView> state)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IClassService>();

            var lessons = await service.GetLessonByClassAsync(ClassId, cancellationTokenSource.Token);
            return new GridData<LessonGridView>()
            {
                Items = lessons,
                TotalItems = lessons.Count
            };
        }
        private void OpenLessonPopup(LessonGridView lessonGridView)
        {
            var lesson = Mapper.Map<LessonRequestModel>(lessonGridView);
            LessonManagementPopup.OpenPopup(lesson);
        }
        private async Task OnSavedAsync()
        {
            await MudDataGrid.ReloadServerData();
        }
    }
}