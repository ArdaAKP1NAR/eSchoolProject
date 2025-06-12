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
    public class ClassService(IMapper mapper, IClassRepository classRepository, ISchoolRepository schoolRepository) : IClassService
    {
        public async Task AddClassAsync(ClassRequestModel classRequestModel, long schoolId, CancellationToken cancellationToken)
        {
            var classToAdd = mapper.Map<Class>(classRequestModel);
            var school = await schoolRepository.GetByIdAsync(schoolId) ?? throw new SchoolNotFoundException("School not found. ");
            if (await classRepository.GetAll().AnyAsync(a => a.ClassLevel == classToAdd.ClassLevel))
            {
                throw new InvalidNameException("This class already exist");
            }
            await classRepository.AddAsync(classToAdd, cancellationToken);
        }
        public async Task<List<ClassViewModel>> GetClassesBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await classRepository.GetAll(cancellationToken)
                .Where(a => a.SchoolId == schoolId)
                .ProjectTo<ClassViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
