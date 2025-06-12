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

            try
            {
                await lessonRepository.AddAsync(lesson, cancellationToken);
            }
            catch (Exception e ) 
            {
                throw;
            }
        }
        public async Task<List<LessonViewModel>> GetLessonBySchoolAsync(long classId, CancellationToken cancellationToken)
        {
            var classEntity = await classRepository.GetAll()
                .Where(a => a.Id == classId)
                .Select(a=> a.SchoolId)
                .FirstAsync(cancellationToken);

            return await lessonRepository.GetAll()
                .Where(a => a.ClassList.Any(b => b.SchoolId == classEntity))
                .ProjectTo<LessonViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
