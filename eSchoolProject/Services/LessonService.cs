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
    public class LessonService(IMapper mapper, ILessonRepository lessonRepository, IClassRepository classRepository) : ILessonService
    {
        public async Task AddLessonAsync(LessonRequestModel lessonRequestModel,List<ClassViewModel> classList, CancellationToken cancellationToken)
        {
            var lesson = mapper.Map<Lesson>(lessonRequestModel);
            var lessonToAddClassEntities = mapper.Map<List<Class>>(classList);

            lesson.ClassList.AddRange(lessonToAddClassEntities);
            await lessonRepository.AddAsync(lesson, cancellationToken);
        }
        public async Task<List<LessonViewModel>> GetLessonBySchoolAsync(long classId, CancellationToken cancellationToken)
        {
            var classEntity = await classRepository.GetAll()
                .Where(a => a.Id == classId)
                .Select(a=> a.SchoolId)
                .FirstAsync();

            return await lessonRepository.GetAll()
                .Where(a => a.ClassList.Any(b => b.SchoolId == classEntity))
                .ProjectTo<LessonViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
