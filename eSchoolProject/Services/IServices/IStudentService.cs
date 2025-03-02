using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface IStudentService
    {
        Task AddStudentAsync(StudentRequestModel studentRequestModel, long schoolId, CancellationToken cancellationToken);
        Task<List<StudentViewModel>> GetStudentBySchoolAsync(long schoolId, CancellationToken cancellationToken);
    }
}