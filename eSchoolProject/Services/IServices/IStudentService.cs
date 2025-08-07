using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface IStudentService
    {
        Task AddStudentAsync(StudentRequestModel studentRequestModel, CancellationToken cancellationToken);
        Task<List<GradeViewModel>> GetGradesByLessonAndStudentAsync(long lessonId, List<long> studentIds, CancellationToken cancellationToken);
        Task UpdateStudentAsync(StudentRequestModel studentRequestModel, CancellationToken cancellationToken);
    }
}