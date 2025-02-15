using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface ISchoolService
    {
        Task AddSchoolAsync(SchoolRequestModel schoolRequestModel, CancellationToken cancellationToken);
        Task<List<SchoolViewModel>> GetAllSchoolsAsync();
    }
}