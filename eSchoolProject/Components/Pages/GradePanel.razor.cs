using eSchoolDatabase;
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

            var teacher = await teacherService.GetTeacherByIdAsync(TeacherId, CancellationTokenSource.Token);

            ClassList = await teacherService.GetClassesByTeacherIdAsync(TeacherId, CancellationTokenSource.Token);
            Students = await teacherService.GetStudentsByTeacherIdAsync(TeacherId, CancellationTokenSource.Token);
            Lessons = await teacherService.GetLessonByTeacherAsync(TeacherId, CancellationTokenSource.Token);
        }
        private async Task OnSelectedLessonChanged(LessonViewModel lesson)
        {
            SelectedLesson = lesson;
            await LoadGradesForSelectedLessonAsync();
        }
        private async Task LoadGradesForSelectedLessonAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var studentService = scope.ServiceProvider.GetRequiredService<IStudentService>();

            var studentIds = SelectedClassStudents.Select(s => s.Id).ToList();

            var grades = await studentService.GetGradesByLessonAndStudentAsync(SelectedLesson.Id, studentIds, CancellationTokenSource.Token);

            foreach (var student in SelectedClassStudents)
            {
                var studentGrades = grades.Where(g => g.StudentId == student.Id).ToList();

                student.Midterm = studentGrades.FirstOrDefault(g => g.GradeType == GradeType.Midterm)?.GradeValue;
                student.Final = studentGrades.FirstOrDefault(g => g.GradeType == GradeType.Final)?.GradeValue;
                student.Oral = studentGrades.FirstOrDefault(g => g.GradeType == GradeType.Oral)?.GradeValue;
                student.Homework = studentGrades.FirstOrDefault(g => g.GradeType == GradeType.Homework)?.GradeValue;
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
                Snackbar.Add("Girilecek not bulunamadý.", Severity.Warning);
                return;
            }

            using var scope = ServiceScopeFactory.CreateScope();
            var gradeService = scope.ServiceProvider.GetRequiredService<IGradeService>();

            foreach (var grade in gradesToSave)
            {
                await gradeService.AddOrUpdateGradeAsync(grade, CancellationTokenSource.Token);
            }

            Snackbar.Add("Notlar baþarýyla kaydedildi.", Severity.Success);
        }
    }
}