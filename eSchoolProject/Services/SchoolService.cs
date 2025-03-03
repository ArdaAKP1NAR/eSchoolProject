using AutoMapper;
using AutoMapper.QueryableExtensions;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;
using eSchoolProject.Exceptions;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace eSchoolProject.Services
{
    public class SchoolService(IMapper mapper, ISchoolRepository schoolRepository) : ISchoolService
    {
        public async Task AddSchoolAsync(SchoolRequestModel schoolRequestModel, CancellationToken cancellationToken)
        {
            var school = mapper.Map<School>(schoolRequestModel);

            if (await schoolRepository.GetAll().AnyAsync(x => x.Name == schoolRequestModel.Name,cancellationToken ))
            {
                throw new InvalidSchoolNameException("This school name exists. ");
            }
            await schoolRepository.AddAsync(school, cancellationToken);
        }
        public async Task<List<SchoolViewModel>> GetAllSchoolsAsync(CancellationToken cancellationToken)
        {
            return await schoolRepository.GetAll().ProjectTo<SchoolViewModel>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
        public async Task<SchoolViewModel> GetSchoolByIdAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await schoolRepository.GetAll()
                .Include(a=> a.Address)
                .ProjectTo<SchoolViewModel>(mapper.ConfigurationProvider)
                .SingleAsync(a => a.Id == schoolId, cancellationToken);
        }
    }
}
