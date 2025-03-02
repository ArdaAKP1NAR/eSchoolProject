using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface IClassService
    {
        Task AddClassAsync(ClassRequestModel classRequestModel, long schoolId, CancellationToken cancellationToken);
        Task<List<ClassViewModel>> GetClassesBySchoolAsync(long schoolId, CancellationToken cancellationToken);
    }
}