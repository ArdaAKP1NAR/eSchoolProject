using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface IGradeService
    {
        Task<List<GradeViewModel>> GetGradesByLessonAndStudentAsync(long lessonId, List<long> studentIds, CancellationToken cancellationToken);
        Task<List<StudentViewModel>> GetStudentsByClassAndTeacherAsync(long classId, long teacherId, CancellationToken cancellationToken);
        Task<List<StudentViewModel>> LoadGradesForSelectedLesson(long classId, long lessonId, long teacherId, CancellationToken cancellationToken);
        Task SaveGradesAsync(List<GradeInputModel> grades, CancellationToken cancellationToken);
    }
}