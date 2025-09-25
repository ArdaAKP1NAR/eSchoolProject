using AutoMapper;
using AutoMapper.QueryableExtensions;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace eSchoolProject.Services
{
    public class LessonScheduleService(ILessonScheduleRepository lessonScheduleRepository, IMapper mapper, IClassRepository classRepository, ILessonRepository lessonRepository, ITeacherRepository teacherRepository) : ILessonScheduleService
    {
        public async Task<List<LessonScheduleViewModel>> GetSchedulesAsync(long? classId, long? teacherId)
        {
            var query = lessonScheduleRepository.GetAll()
                .Include(ls => ls.Class)
                .Include(ls => ls.Lesson)
                .Include(ls => ls.Teacher)
                .AsQueryable();

            if (classId.HasValue)
            {
                query = query.Where(ls => ls.ClassId == classId.Value);
            }
            if (teacherId.HasValue)
            {
                query = query.Where(ls => ls.TeacherId == teacherId.Value);
            }

            return await query
                .ProjectTo<LessonScheduleViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task<(bool Success, string? ErrorMessage)> AddOrUpdateScheduleAsync(LessonScheduleRequestModel request, CancellationToken cancellationToken)
        {
            // 1. Mevcut Id’ye göre güncelleme
            if (request.Id > 0)
            {
                var existingById = await lessonScheduleRepository.GetByIdAsync(request.Id, cancellationToken);
                if (existingById != null)
                {
                    mapper.Map(request, existingById);
                    await lessonScheduleRepository.UpdateAsync(existingById, cancellationToken);
                    await lessonScheduleRepository.SaveChangesAsync(cancellationToken);
                    return (true, null);
                }
            }

            // 2. Sınıfın o saatte başka dersi var mı kontrol
            var existingClassSchedule = await lessonScheduleRepository.GetAll()
                .FirstOrDefaultAsync(s =>
                    s.ClassId == request.ClassId &&
                    s.Day == request.Day &&
                    s.StartTime == request.StartTime,
                    cancellationToken);

            if (existingClassSchedule != null)
            {
                mapper.Map(request, existingClassSchedule);
                await lessonScheduleRepository.UpdateAsync(existingClassSchedule, cancellationToken);
            }
            else
            {
                // 3. Öğretmenin aynı saatte başka dersi var mı kontrol
                var existingTeacherSchedule = await lessonScheduleRepository.GetAll()
                    .FirstOrDefaultAsync(s =>
                        s.TeacherId == request.TeacherId &&
                        s.Day == request.Day &&
                        s.StartTime == request.StartTime,
                        cancellationToken);

                if (existingTeacherSchedule != null)
                {
                    // Hata mesajı döndür
                    return (false, "Seçilen öğretmenin o saatte başka bir dersi var.");
                }

                // 4. Yeni ekleme
                var entity = mapper.Map<LessonSchedule>(request);
                await lessonScheduleRepository.AddAsync(entity, cancellationToken);
            }

            await lessonScheduleRepository.SaveChangesAsync(cancellationToken);
            return (true, null);
        }

    }
}
