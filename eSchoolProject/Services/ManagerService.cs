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
    public class ManagerService(IMapper mapper, ITransactionService transactionService, ILoginService loginService, IManagerRepository managerRepository) : IManagerService
    {
        public async Task AddManagerAsync(ManagerRequestModel managerRequestModel, CancellationToken cancellationToken)
        {
            await transactionService.ExecuteAsync(async () =>
            {
                var manager = mapper.Map<Manager>(managerRequestModel);
                await managerRepository.AddAsync(manager, cancellationToken);
                await loginService.AddUserAsync(new()
                {
                    IdentityNumber = manager.IdentityNumber,
                    Roles = [Roles.Manager],
                    Password = PasswordGenerator.GenerateRandomPassword(8),
                }, cancellationToken);
            });
        }
        public async Task<List<ManagerViewModel>> GetManagersBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await managerRepository.GetAll(cancellationToken).Where(a => a.SchoolId == schoolId).ProjectTo<ManagerViewModel>(mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
