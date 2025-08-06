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
    public class LessonService(IMapper mapper, ILessonRepository lessonRepository, ITeacherRepository teacherRepository, IClassRepository classRepository) : ILessonService
    {
        public async Task AddLessonAsync(LessonRequestModel lessonRequestModel,List<ClassViewModel> classList, long teacherId, CancellationToken cancellationToken)
        {
            var lesson = mapper.Map<Lesson>(lessonRequestModel);
            if (await lessonRepository.GetAll(cancellationToken).AnyAsync(a => a.CourseCode == lesson.CourseCode, cancellationToken))
            {
                throw new AlreadyExistException("This lesson already exists.");
            }

            var entityClassList = await classRepository.GetAll(cancellationToken)
                .Where(a => classList.Select(b => b.Id).Contains(a.Id))
                .ToListAsync(cancellationToken);

            lesson.ClassList = entityClassList;

            var teacher = await teacherRepository.GetByIdAsync(teacherId, cancellationToken)
                ?? throw new ItemNotFoundException("Teacher not found.");

            lesson.Teacher = teacher;
            lesson.TeacherId = teacherId;

            await lessonRepository.AddAsync(lesson, cancellationToken);
        }

    }
}
