using AutoMapper;
using AutoMapper.QueryableExtensions;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Exceptions;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace eSchoolProject.Services
{
    public class LessonService(IMapper mapper, ILessonRepository lessonRepository, ITeacherRepository teacherRepository, IClassRepository classRepository) : ILessonService
    {
        public async Task AddLessonAsync(LessonRequestModel lessonRequestModel, CancellationToken cancellationToken)
        {

            if (lessonRequestModel.Teacher == null)
            {
                throw new ArgumentException("expected teacher to be filled but recieved null");
            }
            var lesson = mapper.Map<Lesson>(lessonRequestModel);    
            if (await lessonRepository.GetAll().AnyAsync(a => a.CourseCode == lesson.CourseCode, cancellationToken))
            {
                throw new LessonAlreadyExistsException("A lesson with the same course code already exists.");
            }

            var entityClassList = await classRepository.GetAll()
                .Where(a => lessonRequestModel.ClassList.Select(b => b.Id).Contains(a.Id))
                .ToListAsync(cancellationToken);

            lesson.Teacher = null; // 🚀 EKLE — EF artık öğretmeni yeniden eklemeye çalışmaz

            lesson.ClassList = entityClassList;

            lesson.TeacherId = lessonRequestModel.Teacher!.Id;

            await lessonRepository.AddAsync(lesson, cancellationToken);
        }
        public async Task UpdateLessonAsync(LessonRequestModel lessonRequestModel, CancellationToken cancellationToken)
        {
            var lesson = await lessonRepository.GetAll()
                .Include(a => a.ClassList)
                .FirstOrDefaultAsync(a => a.Id == lessonRequestModel.Id, cancellationToken);

            if (lesson == null)
                throw new ItemNotFoundException("Lesson not found.");
            
            mapper.Map(lessonRequestModel, lesson);
            
            lesson.TeacherId = lessonRequestModel.Teacher!.Id;

            var entityClassList = await classRepository.GetAll()
                .Where(a => lessonRequestModel.ClassList.Select(b => b.Id).Contains(a.Id))
                .ToListAsync(cancellationToken);

            lesson.ClassList = entityClassList;

            await lessonRepository.SaveChangesAsync(cancellationToken);
        }

    }
}
