using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;
using eSchoolDatabase.ViewModels.GridViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface ISchoolService
    {
        Task AddSchoolAsync(SchoolRequestModel schoolRequestModel, CancellationToken cancellationToken);
        Task<List<SchoolGridView>> GetAllSchoolsAsync(CancellationToken cancellationToken);
        Task<SchoolViewModel> GetSchoolByIdAsync(long schoolId, CancellationToken cancellationToken);
    }
}