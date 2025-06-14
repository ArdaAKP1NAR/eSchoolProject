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
    public class ClassService(IMapper mapper, IStudentRepository studentRepository, IClassRepository classRepository, ISchoolRepository schoolRepository) : IClassService
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
        public async Task<List<ClassViewModel>> GetClassesBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await classRepository.GetAll(cancellationToken)
                .Where(a => a.SchoolId == schoolId)
                .ProjectTo<ClassViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task<List<StudentViewModel>> GetStudentsByClassAsync(long classId, CancellationToken cancellationToken)
        {
            return await studentRepository.GetAll(cancellationToken)
                .Where(a => a.ClassId == classId)
                .ProjectTo<StudentViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task RemoveStudentFromClassAsync(List<StudentViewModel> studentViews, long classId, CancellationToken cancellationToken)
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
        public async Task AssignStudentsToClassAsync(List<StudentViewModel> students, long classId)
        {
            foreach (var studentVm in students)
            {
                var student = await studentRepository.GetByIdAsync(studentVm.Id)
                              ?? throw new StudentNotFoundException("Student not found.");

                student.ClassId = classId;
                await studentRepository.UpdateAsync(student);
            }
        }

        public async Task<List<StudentViewModel>> GetStudentsWithoutClassBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await studentRepository.GetAll(cancellationToken)
                .Where(a => a.SchoolId == schoolId && a.ClassId == null)
                .ProjectTo<StudentViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
