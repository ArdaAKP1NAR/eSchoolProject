using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface IManagerService
    {
        Task AddManagerAsync(ManagerRequestModel managerRequestModel, CancellationToken cancellationToken);
        Task<List<ManagerViewModel>> GetManagersBySchoolAsync(long schoolId, CancellationToken cancellationToken);
    }
}