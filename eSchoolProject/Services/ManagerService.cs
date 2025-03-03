using AutoMapper;
using AutoMapper.QueryableExtensions;
using eSchool.Utils;
using eSchoolDatabase;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.RequestModel;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Components.Pages.Login;
using eSchoolProject.Exceptions;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace eSchoolProject.Services
{
    public class ManagerService(IMapper mapper, ILoginService loginService, IManagerRepository managerRepository, ISchoolRepository schoolRepository) : IManagerService
    {
        public async Task AddManagerAsync(ManagerRequestModel managerRequestModel, CancellationToken cancellationToken)
        {
            var manager = mapper.Map<Manager>(managerRequestModel);
            await managerRepository.AddAsync(manager, cancellationToken);
            await loginService.AddUserAsync(new()
            {
                IdentityNumber = manager.IdentityNumber,
                Roles = [Roles.Manager],
                Password = PasswordGenerator.GenerateRandomPassword(8),
            }, cancellationToken);
            //buraya transaction gelecek. transaction icin transactionservice yazilacak. Transaction birden fazla database guncellemesi / eklemesi oldugunda kullanilir
        }
        public async Task<List<ManagerViewModel>> GetManagersBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await managerRepository.GetAll(cancellationToken).Where(a => a.SchoolId == schoolId).ProjectTo<ManagerViewModel>(mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
