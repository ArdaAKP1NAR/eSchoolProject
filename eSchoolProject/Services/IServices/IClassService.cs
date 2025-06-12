using eSchoolDatabase.Models;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface IClassService
    {
        Task AddClassAsync(ClassRequestModel classRequestModel, long schoolId, CancellationToken cancellationToken);
        Task<List<ClassViewModel>> GetClassesBySchoolAsync(long schoolId, CancellationToken cancellationToken);
        Task<List<StudentViewModel>> GetStudentsByClassAsync(long classId, CancellationToken cancellationToken);
        Task AddStudentToClass(long classId, List<long> StudentIds, CancellationToken cancellationToken);
        Task RemoveStudentFromClass(long classId, List<StudentViewModel> studentList, CancellationToken cancellationToken);
        Task AssignStudentsToClassAsync(List<long> studentIds, long classId);
        Task<List<StudentViewModel>> GetStudentsWithoutClassBySchoolAsync(long schoolId, CancellationToken cancellationToken);
        Task<ClassViewModel> GetClassByIdAsync(long classId, CancellationToken cancellationToken);
    }
}