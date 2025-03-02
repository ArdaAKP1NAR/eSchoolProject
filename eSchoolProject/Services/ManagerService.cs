using AutoMapper;
using AutoMapper.QueryableExtensions;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Exceptions;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace eSchoolProject.Services
{
    public class ManagerService(IMapper mapper, IManagerRepository managerRepository, ISchoolRepository schoolRepository) : IManagerService
    {
        public async Task AddManagerAsync(ManagerRequestModel managerRequestModel, long schoolId, CancellationToken cancellationToken)
        {
            var manager = mapper.Map<Manager>(managerRequestModel);
            var school = await schoolRepository.GetByIdAsync(schoolId) ?? throw new SchoolNotFoundException("School not found. ");

            if (await managerRepository.GetAll().AnyAsync(a => a.IdentityNumber == manager.IdentityNumber))
            {
                throw new InvalidIdentityNumberException("This manager already exist");
            }
            manager.Password = BCrypt.Net.BCrypt.HashPassword(manager.Password);
            await managerRepository.AddAsync(manager, cancellationToken);
            await schoolRepository.UpdateAsync(school, cancellationToken);
        }
        public async Task<List<ManagerViewModel>> GetManagersBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await managerRepository.GetAll(cancellationToken).Where(a => a.SchoolId == schoolId).ProjectTo<ManagerViewModel>(mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
