using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface IStudentService
    {
        Task AddStudentAsync(StudentRequestModel studentRequestModel, CancellationToken cancellationToken);
        Task<List<StudentViewModel>> GetStudentsBySchoolAsync(long schoolId, CancellationToken cancellationToken);
        Task UpdateStudentAsync(StudentRequestModel studentRequestModel, CancellationToken cancellationToken);
    }
}