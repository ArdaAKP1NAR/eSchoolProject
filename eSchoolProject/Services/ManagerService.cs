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
            using var transaction = transactionService.BeginTransaction();
            
                var manager = mapper.Map<Manager>(managerRequestModel);
                var passwd = PasswordGenerator.GenerateRandomPassword(8);
                await managerRepository.AddAsync(manager, cancellationToken);
                await loginService.AddUserAsync(new()
                {
                    IdentityNumber = manager.IdentityNumber,
                    Roles = [Roles.Manager],
                    Password = passwd,
                }, cancellationToken);
           
            await transaction.CommitAsync();
        }
        public async Task UpdateManagerAsync(ManagerRequestModel managerRequestModel, CancellationToken cancellationToken)
        {
            var manager = mapper.Map<Manager>(managerRequestModel);
            await managerRepository.UpdateAsync(manager, cancellationToken);
        }
    }
}
