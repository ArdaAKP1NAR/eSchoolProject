using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface IStudentService
    {
        Task AddStudentAsync(StudentViewModel studentViewModel, CancellationToken cancellationToken);
        Task AddStudentToClass(long classId, List<long> StudentIds, CancellationToken cancellationToken);
        Task<List<StudentViewModel>> GetStudentsBySchoolAsync(long schoolId, CancellationToken cancellationToken);
        Task UpdateStudentAsync(StudentViewModel studentViewModel, CancellationToken cancellationToken);
        Task<List<StudentViewModel>> GetStudentsByClassAsync(long classId, CancellationToken cancellationToken);
        Task RemoveStudentFromClass(long classId, List<StudentViewModel> studentList, CancellationToken cancellationToken);
    }
}