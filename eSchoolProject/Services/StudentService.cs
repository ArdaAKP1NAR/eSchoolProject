using AutoMapper;
using AutoMapper.QueryableExtensions;
using eSchool.Utils;
using eSchoolDatabase;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Components.Pages.Login;
using eSchoolProject.Exceptions;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace eSchoolProject.Services
{
    public class StudentService(IMapper mapper, ITransactionService transactionService, ILoginService loginService, IStudentRepository studentRepository, IClassRepository classRepository) : IStudentService
    {
        public async Task AddStudentAsync(StudentViewModel studentViewModel, CancellationToken cancellationToken)
        {
            await transactionService.ExecuteAsync(async () =>
            {
                var student = mapper.Map<Student>(studentViewModel);
                await studentRepository.AddAsync(student, cancellationToken);
                await loginService.AddUserAsync(new()
                {
                    IdentityNumber = student.IdentityNumber,
                    Roles = [Roles.Student],
                    Password = PasswordGenerator.GenerateRandomPassword(8),
                }, cancellationToken);
            });
        }
        public async Task<List<StudentViewModel>> GetStudentsBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await studentRepository.GetAll()
                .Where(s => s.SchoolId == schoolId)
                .ProjectTo<StudentViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
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
        public async Task UpdateStudentAsync(StudentViewModel studentViewModel, CancellationToken cancellationToken)
        {
            var student = mapper.Map<Student>(studentViewModel);
            await studentRepository.UpdateAsync(student, cancellationToken);
        }
        public async Task<List<StudentViewModel>> GetStudentsByClassAsync(long classId, CancellationToken cancellationToken)
        {
            return await studentRepository.GetAll(cancellationToken)
                .Where(a => a.ClassId == classId)
                .ProjectTo<StudentViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
