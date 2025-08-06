using AutoMapper;
using AutoMapper.QueryableExtensions;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.RequestModel;
using eSchoolDatabase.ViewModels;
using eSchoolDatabase.ViewModels.GridViewModels;
using eSchoolProject.Exceptions;
using eSchoolProject.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace eSchoolProject.Services
{
    public class SchoolService(IMapper mapper, ISchoolRepository schoolRepository, IStudentRepository studentRepository, IManagerRepository managerRepository, ITeacherRepository teacherRepository, IClassRepository classRepository, ILessonRepository lessonRepository) : ISchoolService
    {
        public async Task AddSchoolAsync(SchoolRequestModel schoolRequestModel, CancellationToken cancellationToken)
        {
            var school = mapper.Map<School>(schoolRequestModel);

            if (await schoolRepository.GetAll().AnyAsync(x => x.Name == schoolRequestModel.Name, cancellationToken))
            {
                throw new InvalidSchoolNameException("This school name exists. ");
            }
            await schoolRepository.AddAsync(school, cancellationToken);
        }
        public async Task<List<SchoolGridView>> GetAllSchoolsAsync(CancellationToken cancellationToken)
        {
            return await schoolRepository.GetAll().ProjectTo<SchoolGridView>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
        }
        public async Task<SchoolViewModel> GetSchoolByIdAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await schoolRepository.GetAll()
                .Include(a => a.Address)
                .ProjectTo<SchoolViewModel>(mapper.ConfigurationProvider)
                .SingleAsync(a => a.Id == schoolId, cancellationToken);
        }
        public async Task<List<StudentViewModel>> GetStudentsBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await studentRepository.GetAll()
                .Where(s => s.SchoolId == schoolId)
                .ProjectTo<StudentViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<TeacherViewModel>> GetTeacherBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await teacherRepository.GetAll(cancellationToken)
                .Where(a => a.SchoolId == schoolId)
                .ProjectTo<TeacherViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task<List<ManagerViewModel>> GetManagersBySchoolAsync(long schoolId, CancellationToken cancellationToken)
        {
            return await managerRepository.GetAll(cancellationToken)
                .Where(a => a.SchoolId == schoolId)
                .ProjectTo<ManagerViewModel>(mapper.ConfigurationProvider).
                ToListAsync();
        }
        public async Task<List<LessonViewModel>> GetLessonBySchoolAsync(long classId, CancellationToken cancellationToken)
        {
            var classEntity = await classRepository.GetAll()
                .Where(a => a.Id == classId)
                .Select(a => a.SchoolId)
                .FirstAsync(cancellationToken);

            return await lessonRepository.GetAll()
                .Where(a => a.ClassList.Any(b => b.SchoolId == classEntity))
                .ProjectTo<LessonViewModel>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
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
