using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface IManagerService
    {
        Task AddManagerAsync(ManagerRequestModel managerRequestModel, long schoolId, CancellationToken cancellationToken);
        Task<List<ManagerViewModel>> GetManagersBySchoolAsync(long schoolId, CancellationToken cancellationToken);
    }
}