using eSchoolDatabase.Repositories;
using eSchoolDatabase.Repositories.Interface;
using eSchoolProject.Services;
using eSchoolProject.Services.IServices;
using System.Reflection;

namespace eSchoolProject
{
    public static class Extensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<ISchoolRepository, SchoolRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<ISchoolService, SchoolService>();
        }
    }
}
