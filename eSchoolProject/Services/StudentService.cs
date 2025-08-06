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
    public class StudentService(IMapper mapper, ITransactionService transactionService, ILoginService loginService, IStudentRepository studentRepository) : IStudentService
    {
        public async Task AddStudentAsync(StudentRequestModel studentRequestModel, CancellationToken cancellationToken)
        {

            using var transaction = transactionService.BeginTransaction();
           
            var student = mapper.Map<Student>(studentRequestModel);
                await studentRepository.AddAsync(student, cancellationToken);
                await loginService.AddUserAsync(new()
                {
                    IdentityNumber = student.IdentityNumber,
                    Roles = [Roles.Student],
                    Password = PasswordGenerator.GenerateRandomPassword(8),
                }, cancellationToken);
            
            await transaction.CommitAsync();
        }      
        public async Task UpdateStudentAsync(StudentRequestModel studentRequestModel, CancellationToken cancellationToken)
        {
            var student = mapper.Map<Student>(studentRequestModel);
            await studentRepository.UpdateAsync(student, cancellationToken);
        }
       
    }
}
