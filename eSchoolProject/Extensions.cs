using eSchoolDatabase.Repositories;
using eSchoolDatabase.Repositories.Interface;
using eSchoolProject.Components.Pages.Login;
using eSchoolProject.Services;
using eSchoolProject.Services.IServices;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            services.AddScoped<ILessonScheduleRepository, LessonScheduleRepository>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<ITransactionService, TransactionService>();
            services.TryAddScoped<ILoginService, LoginService>();
            services.AddScoped<ISchoolService, SchoolService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<ILessonScheduleService, LessonScheduleService>();
            services.AddScoped<IConfirmationService, ConfirmationService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
        }
    }
}
