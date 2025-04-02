using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface ILessonService
    {
        Task AddLessonAsync(LessonRequestModel lessonRequestModel, List<ClassViewModel> classList, CancellationToken cancellationToken);
        Task<List<LessonViewModel>> GetLessonBySchoolAsync(long classId, CancellationToken cancellationToken);
    }
}