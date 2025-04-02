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
    public class LessonService(IMapper mapper, ILessonRepository lessonRepository) : ILessonService
    {
        public async Task AddLessonAsync(LessonRequestModel lessonRequestModel)
        {
            var lesson = mapper.Map<Lesson>(lessonRequestModel);
            await lessonRepository.AddAsync(lesson);
        }
        public async Task<List<LessonViewModel>> GetAllLessons()
        {
            return await lessonRepository.GetAll()
                .ProjectTo<LessonViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
