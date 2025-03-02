using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface ITeacherService
    {
        Task AddTeacherAsync(TeacherRequestModel teacherRequestModel, long schoolId, CancellationToken cancellationToken);
        Task<List<TeacherViewModel>> GetTeacherBySchoolAsync(long schoolId, CancellationToken cancellationToken);
    }
}