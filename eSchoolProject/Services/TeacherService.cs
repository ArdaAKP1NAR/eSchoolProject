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
    public class TeacherService(IMapper mapper, ILoginService loginService, ITeacherRepository teacherRepository, ISchoolRepository schoolRepository) : ITeacherService
    {
        public async Task AddTeacherAsync(TeacherRequestModel teacherRequestModel, CancellationToken cancellationToken)
        {
            var teacher = mapper.Map<Teacher>(teacherRequestModel);
            await teacherRepository.AddAsync(teacher, cancellationToken);
            await loginService.AddUserAsync(new()
            {
                IdentityNumber = teacher.IdentityNumber,
                Roles = [Roles.Teacher],
                Password = PasswordGenerator.GenerateRandomPassword(8),
            }, cancellationToken);
            //buraya transaction gelecek. transaction icin transactionservice yazilacak. Transaction birden fazla database guncellemesi / eklemesi oldugunda kullanilir
        }
        public async Task<List<TeacherViewModel>> GetTeacherBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await teacherRepository.GetAll(cancellationToken)
                .Where(a => a.SchoolId == schoolId)
                .ProjectTo<TeacherViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
