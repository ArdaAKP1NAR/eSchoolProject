using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface ILessonService
    {
        Task AddLessonAsync(LessonRequestModel lessonRequestModel, CancellationToken cancellationToken);
    }
}