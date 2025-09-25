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

            lesson.ClassList = entityClassList;

            lesson.TeacherId = lessonRequestModel.Teacher!.Id;

            await lessonRepository.AddAsync(lesson, cancellationToken);
        }

    }
}
