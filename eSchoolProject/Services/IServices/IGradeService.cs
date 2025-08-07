using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface IGradeService
    {
        Task AddOrUpdateGradeAsync(GradeInputModel gradeInputModel, CancellationToken cancellationToken);
    }
}