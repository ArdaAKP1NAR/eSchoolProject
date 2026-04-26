using eSchoolDatabase;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Services;
using eSchoolProject.Services.IServices;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading;

namespace eSchoolProject.Components.Pages
{
    public partial class GradePanel : IDisposable
    {
        [Inject] private IServiceScopeFactory ServiceScopeFactory { get; init; } = default!;
        [Inject] ISnackbar Snackbar { get; init; } = default!;
        [Parameter] public long TeacherId { get; set; }
        private CancellationTokenSource CancellationTokenSource = new();
        private List<ClassViewModel> ClassList = new();
        private List<LessonViewModel> Lessons = new();
        private LessonViewModel SelectedLesson = default!;
        private List<StudentViewModel> SelectedClassStudents = default!;
        private ClassViewModel SelectedClass = default!;
        private List<LessonViewModel> SelectedClassLessons => Lessons.Where(a => a.ClassList.Any(c => c.Id == SelectedClass.Id)).ToList();
        protected override async Task OnInitializedAsync()
        {
            await LoadInitialDataAsync();
            await base.OnInitializedAsync();
        }
        private async Task LoadInitialDataAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var teacherService = scope.ServiceProvider.GetRequiredService<ITeacherService>();

            ClassList = await teacherService.GetClassesByTeacherIdAsync(TeacherId, CancellationTokenSource.Token);
        }
        private async Task OnSelectedLessonChanged(LessonViewModel lesson)
        {
            SelectedLesson = lesson;
            await LoadGradesForSelectedLessonAsync();
        }
        private async Task LoadLessons()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var teacherService = scope.ServiceProvider.GetRequiredService<ITeacherService>();

            SelectedLesson = null;
            SelectedClassStudents = new();

            Lessons = await teacherService.GetLessonByTeacherAsync(TeacherId, SelectedClass.Id, CancellationTokenSource.Token);
        }
        private async Task LoadGradesForSelectedLessonAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var gradeService = scope.ServiceProvider.GetRequiredService<IGradeService>();

            try
            {
                SelectedClassStudents = await gradeService.LoadGradesForSelectedLesson(
                       SelectedClass.Id,
                       SelectedLesson.Id,
                       TeacherId,
                       CancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Hata oluştu: {ex.Message}", Severity.Error);
            }
            StateHasChanged();
        }


        private async Task SaveGradesAsync()
        {
            if (SelectedLesson is null)
            {
                Snackbar.Add("Lütfen önce bir ders seçin.", Severity.Warning);
                return;
            }

            var gradesToSave = new List<GradeInputModel>();

            foreach (var student in SelectedClassStudents)
            {
                var gradeEntries = new (double? Grade, GradeType Type)[]
                {
                    (student.Midterm, GradeType.Midterm),
                    (student.Final, GradeType.Final),
                    (student.Oral, GradeType.Oral),
                    (student.Homework, GradeType.Homework)
                };

                foreach (var (grade, type) in gradeEntries)
                {
                    if (grade.HasValue)
                    {
                        gradesToSave.Add(new GradeInputModel
                        {
                            StudentId = student.Id,
                            LessonId = SelectedLesson.Id,
                            Grade = grade.Value,
                            GradeType = type
                        });
                    }
                }
            }

            if (!gradesToSave.Any())
            {
                Snackbar.Add("Girilecek not bulunamadı.", Severity.Warning);
                return;
            }

            try
            {
                using var scope = ServiceScopeFactory.CreateScope();
                var gradeService = scope.ServiceProvider.GetRequiredService<IGradeService>();

                await gradeService.SaveGradesAsync(gradesToSave, CancellationTokenSource.Token);

                Snackbar.Add("Notlar başarıyla kaydedildi.", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Hata oluştu: {ex.Message}", Severity.Error);
            }
        }
        public void Dispose()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }
    }
}