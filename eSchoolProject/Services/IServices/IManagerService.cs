using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface IManagerService
    {
        Task AddManagerAsync(ManagerRequestModel managerRequestModel, CancellationToken cancellationToken);
    }
}