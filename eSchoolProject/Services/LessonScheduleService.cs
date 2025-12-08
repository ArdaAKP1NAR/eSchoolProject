using AutoMapper;
using AutoMapper.QueryableExtensions;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Exceptions;
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
                    .ThenInclude(l => l.Teacher)
                .AsQueryable();

            if (classId.HasValue)
            {
                query = query.Where(ls => ls.ClassId == classId.Value);
            }

            if (teacherId.HasValue)
            {
                query = query.Where(ls => ls.Lesson.TeacherId == teacherId.Value);
            }

            return await query
                .ProjectTo<LessonScheduleViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task AddOrUpdateScheduleAsync(LessonScheduleRequestModel request, CancellationToken cancellationToken)
        {
            if (request.Teacher == null || request.Lesson == null)
            {
                throw new ArgumentException("expected teacher and lesson to be filled but recieved null");
            }
            if (request.Id > 0)
            {
                var existingById = await lessonScheduleRepository.GetByIdAsync(request.Id, cancellationToken);
                if (existingById != null)
                {
                    mapper.Map(request, existingById);
                    existingById.TeacherId = request.Teacher.Id;
                    existingById.LessonId = request.Lesson.Id;
                    await lessonScheduleRepository.UpdateAsync(existingById, cancellationToken);
                    await lessonScheduleRepository.SaveChangesAsync(cancellationToken);
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
                existingClassSchedule.TeacherId = request.Teacher.Id;
                existingClassSchedule.LessonId = request.Lesson.Id;
                await lessonScheduleRepository.UpdateAsync(existingClassSchedule, cancellationToken);
            }
            else
            {
                // 3. Öğretmenin aynı saatte başka dersi var mı kontrol
                var existingTeacherSchedule = await lessonScheduleRepository.GetAll()
                    .FirstOrDefaultAsync(s =>
                        s.TeacherId == request.Teacher.Id &&
                        s.Day == request.Day &&
                        s.StartTime == request.StartTime,
                        cancellationToken);

                if (existingTeacherSchedule != null)
                {
                    throw new AlreadyExistException("Bu öğretmenin aynı saatte başka bir dersi bulunmaktadır.");
                }

                var entity = mapper.Map<LessonSchedule>(request);
                entity.TeacherId = request.Teacher.Id;
                entity.LessonId = request.Lesson.Id;
                await lessonScheduleRepository.AddAsync(entity, cancellationToken);
            }

            await lessonScheduleRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
