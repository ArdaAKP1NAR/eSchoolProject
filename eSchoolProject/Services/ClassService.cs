using AutoMapper;
using AutoMapper.QueryableExtensions;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Exceptions;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace eSchoolProject.Services
{
    public class ClassService(IMapper mapper, ILessonRepository lessonRepository, IStudentRepository studentRepository, IClassRepository classRepository, ISchoolRepository schoolRepository) : IClassService
    {
        public async Task<ClassViewModel> GetClassByIdAsync(long classId, CancellationToken cancellationToken)
        {
            var classEntity = await classRepository.GetByIdAsync(classId, cancellationToken) ?? throw new ItemNotFoundException("Class not found. ");
            var classView = mapper.Map<ClassViewModel>(classEntity);
            
            return classView;
        }
        public async Task AddClassAsync(ClassRequestModel classRequestModel, long schoolId, CancellationToken cancellationToken)
        {
            var classToAdd = mapper.Map<Class>(classRequestModel);
            var school = await schoolRepository.GetByIdAsync(schoolId) ?? throw new SchoolNotFoundException("School not found. ");
           
            if (await classRepository.GetAll().AnyAsync(a => a.ClassLevel == classToAdd.ClassLevel))
            {
                throw new InvalidNameException("This class already exist");
            }
            
            await classRepository.AddAsync(classToAdd, cancellationToken);
        }

        public async Task<List<StudentViewModel>> GetStudentsByClassAsync(long classId, CancellationToken cancellationToken)
        {
            return await studentRepository.GetAll(cancellationToken)
                .Where(a => a.ClassId == classId)
                .ProjectTo<StudentViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task RemoveStudentFromClassAsync(List<StudentViewModel> studentViews, long classId, CancellationToken cancellationToken) // This method removes a list of students from a specific class
        {
            var selectedStudentIds = studentViews.Select(s => s.Id).ToList();

            var studentsToRemoveClass = await studentRepository.GetAll(cancellationToken)
                .Where(s => s.ClassId == classId && selectedStudentIds.Contains(s.Id))
                .ToListAsync(cancellationToken);

            if (!studentsToRemoveClass.Any())
            {
                throw new ItemNotFoundException("No matching students found in this class.");
            }

            foreach (var student in studentsToRemoveClass)
            {
                student.ClassId = null;
            }

            await studentRepository.UpdateRangeAsync(studentsToRemoveClass, cancellationToken);
        }
        public async Task AssignStudentsToClassAsync(List<StudentViewModel> students, long classId, CancellationToken cancellationToken) // This method assigns a list of students to a specific class
        {
            var studentIds = students.Select(s => s.Id).ToList();

            var studentsToUpdate = await studentRepository.GetAll(cancellationToken)
                .Where(s => studentIds.Contains(s.Id) && s.ClassId == null)
                .ToListAsync(cancellationToken);

            if (studentsToUpdate.Count != studentIds.Count)
            {
                throw new StudentNotFoundException("Some students could not be found in the system.");
            }

            foreach (var student in studentsToUpdate)
            {
                student.ClassId = classId;
            }

            await studentRepository.UpdateRangeAsync(studentsToUpdate, cancellationToken);
        }
        public async Task<List<StudentViewModel>> GetStudentsWithoutClassBySchoolAsync(long schoolId, CancellationToken cancellationToken) // This method retrieves students without a class in a specific school
        {
            return await studentRepository.GetAll(cancellationToken)
                .Where(a => a.SchoolId == schoolId && a.ClassId == null)
                .ProjectTo<StudentViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
        public async Task ReassignStudentsToClassAsync(List<StudentViewModel> students, long classId, CancellationToken cancellationToken)
        {
            var studentIds = students.Select(s => s.Id).ToList();
            
            var studentsToUpdate = await studentRepository.GetAll(cancellationToken)
                .Where(s => studentIds.Contains(s.Id))
                .ToListAsync(cancellationToken);
         
            if (!studentsToUpdate.Any())
            {
                throw new ItemNotFoundException("No matching students found.");
            }
            
            foreach (var student in studentsToUpdate)
            {
                student.ClassId = classId;
            }
            
            await studentRepository.UpdateRangeAsync(studentsToUpdate, cancellationToken);
        }
        public async Task<List<LessonViewModel>> GetLessonByClassAsync(long classId, CancellationToken cancellationToken)
        {
            return await lessonRepository.GetAll(cancellationToken)
                .Where(a => a.ClassList.Any(b => b.Id == classId))
                .ProjectTo<LessonViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
