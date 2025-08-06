using eSchoolDatabase.ViewModels;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class GradePanel
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] ISnackbar Snackbar { get; init; } = default!;
        [Parameter] public long TeacherId { get; set; }
        private CancellationTokenSource CancellationTokenSource = new();
        private List<StudentViewModel> Students = new();
        private List<ClassViewModel> ClassList = new();
        private List<LessonViewModel> Lessons = new();
        protected override async Task OnInitializedAsync()
        {
            await LoadInitialDataAsync();
            await base.OnInitializedAsync();
        }
        private async Task LoadInitialDataAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var teacherService = scope.ServiceProvider.GetRequiredService<ITeacherService>();

            var teacher = await teacherService.GetTeacherByIdAsync(TeacherId, CancellationTokenSource.Token);
            
            ClassList = await teacherService.GetClassesByTeacherIdAsync(TeacherId, CancellationTokenSource.Token);
            Students = await teacherService.GetStudentsByTeacherIdAsync(TeacherId, CancellationTokenSource.Token);
            Lessons = await teacherService.GetLessonByTeacherAsync(TeacherId, CancellationTokenSource.Token);
        }
    }
}