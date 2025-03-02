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
    public class TeacherService(IMapper mapper, ITeacherRepository teacherRepository, ISchoolRepository schoolRepository) : ITeacherService
    {
        public async Task AddTeacherAsync(TeacherRequestModel teacherRequestModel, long schoolId, CancellationToken cancellationToken)
        {
            var teacher = mapper.Map<Teacher>(teacherRequestModel);
            var school = await schoolRepository.GetByIdAsync(schoolId) ?? throw new SchoolNotFoundException("School not found. ");
            if (await teacherRepository.GetAll().AnyAsync(a => a.IdentityNumber == teacher.IdentityNumber))
            {
                throw new InvalidIdentityNumberException("This teacher already exist");
            }
            teacher.Password = BCrypt.Net.BCrypt.HashPassword(teacher.Password);
            await teacherRepository.AddAsync(teacher);
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
