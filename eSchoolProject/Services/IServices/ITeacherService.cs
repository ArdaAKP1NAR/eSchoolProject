using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface ITeacherService
    {
        Task<List<LessonViewModel>> GetLessonByTeacherAsync(long teacherId, long classId, CancellationToken cancellationToken);
        Task AddTeacherAsync(TeacherRequestModel teacherRequestModel, CancellationToken cancellationToken);
        Task<List<ClassViewModel>> GetClassesByTeacherIdAsync(long teacherId, CancellationToken cancellationToken);
        Task<List<StudentViewModel>> GetStudentsByTeacherIdAsync(long teacherId, CancellationToken cancellationToken);
        Task<TeacherViewModel> GetTeacherByIdAsync(long teacherId, CancellationToken cancellationToken);
        Task UpdateTeacherAsync(TeacherRequestModel teacherRequestModel, CancellationToken cancellationToken);
    }
}