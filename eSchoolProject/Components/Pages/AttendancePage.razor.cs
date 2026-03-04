using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace eSchoolProject.Components.Pages
{
    public partial class AttendancePage
    {
        [Parameter] public long TeacherId { get; set; }
        [Parameter] public long SchoolId { get; set; }
        private CancellationTokenSource CancellationTokenSource = new();
        private DateTime? SelectedDate = DateTime.Today;
        private long SelectedClassId;
        private List<ClassViewModel> TeacherClasses = new();
        private List<LessonScheduleViewModel>? Schedules;
        private LessonScheduleViewModel? SelectedSchedule;
        private List<AttendanceViewModel>? StudentAttendances;

        protected override async Task OnInitializedAsync()
        {
            if (TeacherId > 0)
            {
                TeacherClasses = await AttendanceService.GetTeacherClassesAsync(TeacherId);
            }
        }

        private async Task LoadSchedules()
        {
            if (SelectedDate.HasValue && TeacherId > 0 && SelectedClassId > 0)
            {
                Schedules = await AttendanceService.GetLessonSchedulesAsync(TeacherId, SelectedClassId, SelectedDate.Value);
                SelectedSchedule = null;
                StudentAttendances = null;
            }
            else if (SelectedClassId == 0)
            {
                Snackbar.Add("Lütfen önce bir sınıf seçiniz.", Severity.Warning);
            }
        }

        private async Task SelectSchedule(LessonScheduleViewModel schedule)
        {
            SelectedSchedule = schedule;
            if (SelectedDate.HasValue)
            {
                StudentAttendances = await AttendanceService.GetStudentAttendanceListAsync(schedule.Id, SelectedDate.Value);
            }
        }

        private async Task SaveAttendance()
        {
            if (StudentAttendances == null || !StudentAttendances.Any()) return;

            var requestModels = StudentAttendances.Select(s => new AttendanceRequestModel
            {
                StudentId = s.StudentId,
                LessonId = s.LessonId,
                AttendanceDate = s.AttendanceDate,
                IsPresent = s.IsPresent
            }).ToList();

            var result = await AttendanceService.SaveAttendanceAsync(requestModels, CancellationTokenSource.Token);

            if (result)
            {
                Snackbar.Add("Yoklama başarıyla kaydedildi.", Severity.Success);
                await SelectSchedule(SelectedSchedule!);
            }
            else
            {
                Snackbar.Add("Kayıt sırasında bir hata oluştu.", Severity.Error);
            }
        }

        private void GoBack()
        {
            NavigationManager.NavigateTo($"/SchoolManagement/{SchoolId}");
        }
    }
}