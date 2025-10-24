using AutoMapper;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Components.PopupComponent;
using eSchoolProject.Services;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq;
using System.Threading;

namespace eSchoolProject.Components.Pages
{
    public partial class LessonScheduleManagement : IDisposable
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] ISnackbar Snackbar { get; init; } = default!;
        [Inject] IMapper Mapper { get; init; } = default!;
        [Parameter] public long SchoolId { get; set; }
        private CancellationTokenSource CancellationTokenSource = new();
        private List<ClassViewModel> ClassList = new();
        private List<TeacherViewModel> Teachers = new();
        private List<LessonViewModel> Lessons = new();
        private long? selectedClassId;
        private long? selectedTeacherId;
        private List<LessonScheduleViewModel> FilteredSchedules = new();
        private LessonSchedulePopup schedulePopup = default!; 
        private List<TimeSpan> TimeSlots = Enumerable.Range(8, 10).Select(h => new TimeSpan(h, 0, 0)).ToList();

        protected override async Task OnInitializedAsync()
        {
            await LoadSchedules();
            if (ClassList.Any())
            {
                selectedClassId = ClassList.First().Id;
                await OnFilterChanged(); // Seçime göre grid’i filtrele
            }
            await base.OnInitializedAsync();
        }
        private async Task LoadSchedules()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var lessonScheduleService = scope.ServiceProvider.GetRequiredService<ILessonScheduleService>();
            var schoolService = scope.ServiceProvider.GetRequiredService<ISchoolService>();
            ClassList = await schoolService.GetClassesBySchoolAsync(SchoolId ,CancellationTokenSource.Token);
            Teachers = await schoolService.GetTeacherBySchoolAsync(SchoolId, CancellationTokenSource.Token);
            Lessons = await schoolService.GetLessonBySchoolAsync(SchoolId, CancellationTokenSource.Token);
            FilteredSchedules = await lessonScheduleService.GetSchedulesAsync(selectedClassId, selectedTeacherId);
        }
        private async Task OnFilterChanged()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var lessonScheduleService = scope.ServiceProvider.GetRequiredService<ILessonScheduleService>();
            FilteredSchedules = await lessonScheduleService.GetSchedulesAsync(selectedClassId, selectedTeacherId);
            StateHasChanged();
        }
        private async Task OnScheduleSaved()
        {
            await OnFilterChanged();
            Snackbar.Add("Ders programý kaydedildi.", Severity.Success);
        }
        private void OpenSchedulePopupFromViewModel(LessonScheduleViewModel lessonScheduleViewModel)
        {
            if (lessonScheduleViewModel != null)
            {
                // AutoMapper kullanarak dönüþtür
                var lessonScheduleRequestModel = Mapper.Map<LessonScheduleRequestModel>(lessonScheduleViewModel);
                schedulePopup.Open(lessonScheduleRequestModel);
            }
        }
        private void OpenSchedulePopupForNew(DayOfWeek day, TimeSpan startTime)
        {
            var newRequest = new LessonScheduleRequestModel
            {
                ClassId = selectedClassId ?? 0, // Seçili sýnýfý ata
                Day = day,
                StartTime = startTime,
                EndTime = startTime.Add(TimeSpan.FromHours(1)) // 1 saatlik blok
            };
            schedulePopup.Open(newRequest);
        }
        public void Dispose()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }
    }
}