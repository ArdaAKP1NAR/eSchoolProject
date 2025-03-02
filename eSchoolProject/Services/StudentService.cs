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
    public class StudentService(IMapper mapper, IStudentRepository studentRepository, ISchoolRepository schoolRepository) : IStudentService
    {
        public async Task AddStudentAsync(StudentRequestModel studentRequestModel, long schoolId, CancellationToken cancellationToken)
        {
            var student = mapper.Map<Student>(studentRequestModel);
            var school = await schoolRepository.GetByIdAsync(schoolId) ?? throw new SchoolNotFoundException("School not found. ");
            if (await studentRepository.GetAll().AnyAsync(a => a.IdentityNumber == student.IdentityNumber))
            {
                throw new InvalidIdentityNumberException("This teacher already exist");
            }
            student.Password = BCrypt.Net.BCrypt.HashPassword(student.Password);
            await studentRepository.AddAsync(student);
        }
        public async Task<List<StudentViewModel>> GetStudentBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await schoolRepository.GetAll()
                .Where(a => a.Id == schoolId)
                .ProjectTo<StudentViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
