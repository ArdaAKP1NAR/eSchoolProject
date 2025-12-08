using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Services;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;

namespace eSchoolProject.Components.PopupComponent
{
    public partial class LessonSchedulePopup
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] private ISnackbar Snackbar { get; init; } = default!;
        [Parameter] public bool IsLessonSchedulePopupVisible { get; set; }
        [Parameter] public EventCallback ClosePopup { get; set; }
        [Parameter] public EventCallback SaveClicked { get; set; }
        [Parameter] public LessonScheduleRequestModel RequestModel { get; set; } = default!;
        [Parameter] public List<ClassViewModel> ClassList { get; set; } = new();
        [Parameter] public List<TeacherViewModel> Teachers { get; set; } = new();
        [Parameter] public List<LessonViewModel> Lessons { get; set; } = new();
        public List<LessonViewModel> LessonsForSelectedTeacher
            => RequestModel?.Teacher != null
               ? Lessons.Where(a => a.Teacher.Id == RequestModel.Teacher.Id && a.ClassList.Any(c => c.Id == RequestModel.ClassId)).ToList()
               : new List<LessonViewModel>();

        public List<TeacherViewModel> TeachersForSelectedClass
            => RequestModel?.ClassId > 0
                ? Teachers.Where(t => Lessons.Any(l => l.Teacher.Id == t.Id && l.ClassList.Any(c => c.Id == RequestModel.ClassId))).ToList()
                : new List<TeacherViewModel>();

        private CancellationTokenSource cancellationTokenSource = new();

        public void Open(LessonScheduleRequestModel model)
        {
            RequestModel = model;
            IsLessonSchedulePopupVisible = true;
        }

        private async Task SaveScheduleAsync()
        {
            if (RequestModel != null)
            {
                // Gün ve saat zaten modelde mevcut, sadece servis çaðrýsý
                using var scope = ServiceScopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<ILessonScheduleService>();

                try
                {
                    await service.AddOrUpdateScheduleAsync(RequestModel, cancellationTokenSource.Token);
                    await SaveClicked.InvokeAsync(cancellationTokenSource.Token);
                    Snackbar.Add("Ders programý kaydedildi.", Severity.Success);
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"An error occurred: {ex.Message}", Severity.Error);
                    return;
                }

                IsLessonSchedulePopupVisible = false;
            }
        
        }

        private async Task ClosePopupAsync()
        {
            IsLessonSchedulePopupVisible = false;
            await ClosePopup.InvokeAsync();
        }
    }
}