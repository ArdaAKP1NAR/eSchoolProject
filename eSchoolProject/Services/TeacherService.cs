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
    public class TeacherService(IMapper mapper, ILessonRepository lessonRepository, IStudentRepository studentRepository, IClassRepository classRepository, ITransactionService transactionService, ILoginService loginService, ITeacherRepository teacherRepository) : ITeacherService
    {
        public async Task AddTeacherAsync(TeacherRequestModel teacherRequestModel, CancellationToken cancellationToken)
        {

            using var transaction = transactionService.BeginTransaction();

            var teacher = mapper.Map<Teacher>(teacherRequestModel);
            await teacherRepository.AddAsync(teacher, cancellationToken);
            await loginService.AddUserAsync(new()
            {
                IdentityNumber = teacher.IdentityNumber,
                Roles = [Roles.Teacher],
                Password = PasswordGenerator.GenerateRandomPassword(8),
            }, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        public async Task UpdateTeacherAsync(TeacherRequestModel teacherRequestModel, CancellationToken cancellationToken)
        {
            var teacher = mapper.Map<Teacher>(teacherRequestModel);
            await teacherRepository.UpdateAsync(teacher, cancellationToken);
        }
        public async Task<List<LessonViewModel>> GetLessonByTeacherAsync(long teacherId, long classId, CancellationToken cancellationToken)
        {
            return await lessonRepository.GetAll()
                .Where(a => a.TeacherId == teacherId &&
                       a.ClassList.Any(a => a.Id == classId))
                .ProjectTo<LessonViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
        public async Task<TeacherViewModel> GetTeacherByIdAsync(long teacherId, CancellationToken cancellationToken)
        {
            return await teacherRepository.GetAll()
             .ProjectTo<TeacherViewModel>(mapper.ConfigurationProvider)
             .SingleAsync(a => a.Id == teacherId, cancellationToken);
        }
        public async Task<List<ClassViewModel>> GetClassesByTeacherIdAsync(long teacherId, CancellationToken cancellationToken)
        {
            return await lessonRepository.GetAll()
                .Where(l => l.TeacherId == teacherId)
                .SelectMany(l => l.ClassList)
                .Distinct() // Aynı sınıf birden fazla derste olabilir, tekrarları engelle
                .ProjectTo<ClassViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<StudentViewModel>> GetStudentsByTeacherIdAsync(long teacherId, CancellationToken cancellationToken)
        {
            var classList = await lessonRepository.GetAll()
                .Where(l => l.TeacherId == teacherId)
                .SelectMany(l => l.ClassList)
                .Distinct()
                .ToListAsync(cancellationToken);

            var classIds = classList.Select(c => c.Id).ToList();

            return await studentRepository.GetAll()
                .Where(s => s.ClassId != null && classIds.Contains(s.ClassId.Value))
                .ProjectTo<StudentViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

    }
}
