using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface ILessonService
    {
        Task AddLessonAsync(LessonRequestModel lessonRequestModel, List<ClassViewModel> classList, long teacherId, CancellationToken cancellationToken);
    }
}