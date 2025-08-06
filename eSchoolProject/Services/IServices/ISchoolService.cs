using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;
using eSchoolDatabase.ViewModels.GridViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface ISchoolService
    {
        Task AddSchoolAsync(SchoolRequestModel schoolRequestModel, CancellationToken cancellationToken);
        Task<List<SchoolGridView>> GetAllSchoolsAsync(CancellationToken cancellationToken);
        Task<List<ClassViewModel>> GetClassesBySchoolAsync(long schoolId, CancellationToken cancellationToken);
        Task<List<LessonViewModel>> GetLessonBySchoolAsync(long classId, CancellationToken cancellationToken);
        Task<List<ManagerViewModel>> GetManagersBySchoolAsync(long schoolId, CancellationToken cancellationToken);
        Task<SchoolViewModel> GetSchoolByIdAsync(long schoolId, CancellationToken cancellationToken);
        Task<List<StudentViewModel>> GetStudentsBySchoolAsync(long schoolId, CancellationToken cancellationToken);
        Task<List<TeacherViewModel>> GetTeacherBySchoolAsync(long schoolId, CancellationToken cancellationToken);
    }
}