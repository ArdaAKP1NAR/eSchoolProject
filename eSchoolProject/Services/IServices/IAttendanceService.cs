using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eSchoolProject.Services.IServices
{
    public interface IAttendanceService
    {
        Task<List<ClassViewModel>> GetTeacherClassesAsync(long teacherId);
        Task<List<LessonScheduleViewModel>> GetLessonSchedulesAsync(long teacherId, long classId, DateTime date);
        Task<List<AttendanceViewModel>> GetStudentAttendanceListAsync(long lessonScheduleId, DateTime date);
        Task<bool> SaveAttendanceAsync(List<AttendanceRequestModel> models, CancellationToken cancellationToken);
    }
}
