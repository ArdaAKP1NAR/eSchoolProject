using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Services.IServices
{
    public interface ILessonScheduleService
    {
        Task<(bool Success, string? ErrorMessage)> AddOrUpdateScheduleAsync(LessonScheduleRequestModel request, CancellationToken cancellationToken);
        Task<List<LessonScheduleViewModel>> GetSchedulesAsync(long? classId, long? teacherId);
    }
}