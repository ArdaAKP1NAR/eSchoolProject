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
        public async Task AddStudentToClass(long classId, List<long> StudentIds, CancellationToken cancellationToken)
        {
            var students = await studentRepository.GetAll()
                .Where(s => StudentIds.Contains(s.Id))
                .ToListAsync(cancellationToken);
            foreach (var student in students)
            {
                student.ClassId = classId;
            }
            await studentRepository.UpdateRangeAsync(students, cancellationToken);
        }
        public async Task RemoveStudentFromClass(long classId, List<StudentViewModel> studentList, CancellationToken cancellationToken)
        {
            var studentsToRemoveClass = studentList
                .Where(s => s.ClassId == classId)
                .ToList();

            var studentsToRemoveClassEntities = mapper.Map<List<Student>>(studentsToRemoveClass);

            foreach (var student in studentsToRemoveClassEntities)
            {
                student.ClassId = null;
            }

            if (studentsToRemoveClassEntities.Any())
            {
                await studentRepository.UpdateRangeAsync(studentsToRemoveClassEntities, cancellationToken);
            }
        }
        public async Task AssignStudentsToClassAsync(List<long> studentIds, long classId)
        {
            foreach (var id in studentIds)
            {
                var student = await studentRepository.GetByIdAsync(id) ?? throw new StudentNotFoundException("Student not found. ");
                student.ClassId = classId;
                await studentRepository.UpdateAsync(student);
            }
        }
        public async Task<List<StudentViewModel>> GetStudentsWithoutClassBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await studentRepository.GetAll(cancellationToken)
                .Where(a => a.SchoolId == schoolId && a.ClassId == 0)
                .ProjectTo<StudentViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        }
    }
}
