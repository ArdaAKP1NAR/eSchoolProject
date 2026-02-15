using AutoMapper;
using eSchoolDatabase.Models;
using eSchoolDatabase.RequestModel;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;
using eSchoolDatabase.ViewModels.GridViewModels;

namespace eSchoolProject.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<School, SchoolRequestModel>().ReverseMap();
            CreateMap<School, SchoolViewModel>().ReverseMap();
            CreateMap<SchoolRequestModel, SchoolViewModel>().ReverseMap();
            CreateMap<School, SchoolGridView>().ReverseMap();

            CreateMap<Address, AddressRequestModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
            CreateMap<AddressRequestModel, AddressViewModel>().ReverseMap();

            CreateMap<Manager, ManagerRequestModel>().ReverseMap();
            CreateMap<Manager, ManagerViewModel>().ReverseMap();
            CreateMap<Manager, ManagerGridView>().ReverseMap();
            CreateMap<ManagerRequestModel, ManagerGridView>().ReverseMap();

            CreateMap<Teacher, TeacherRequestModel>().ReverseMap();
            CreateMap<Teacher, TeacherViewModel>().ReverseMap();
            CreateMap<Teacher, TeacherGridView>().ReverseMap();
            CreateMap<TeacherRequestModel, TeacherGridView>().ReverseMap();

            CreateMap<Student, StudentRequestModel>().ReverseMap();
            CreateMap<Student, StudentViewModel>().ReverseMap();
            CreateMap<StudentRequestModel, StudentViewModel>().ReverseMap();
            CreateMap<Student, StudentGridView>().ReverseMap();
            CreateMap<StudentRequestModel, StudentGridView>().ReverseMap();

            CreateMap<Class, ClassViewModel>().ReverseMap();
            CreateMap<Class, ClassRequestModel>().ReverseMap();
            CreateMap<ClassViewModel, ClassRequestModel>().ReverseMap();
            CreateMap<Class, ClassGridView>().ReverseMap();

            CreateMap<Lesson, LessonViewModel>().ReverseMap();
            CreateMap<Lesson, LessonRequestModel>().ReverseMap()
                .ForMember(dest => dest.ClassList, opt => opt.Ignore())
                .ForMember(dest => dest.Teacher, opt => opt.Ignore());
            CreateMap<LessonRequestModel, LessonGridView>().ReverseMap();
            CreateMap<Lesson, LessonGridView>().ReverseMap();

            CreateMap<Grade, GradeViewModel>().ReverseMap();
            CreateMap<Grade, GradeInputModel>().ReverseMap();

            CreateMap<LessonSchedule, LessonScheduleViewModel>()
               .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.TeacherId)) // Ensures direct mapping
               .ForMember(dest => dest.StartTimeStr, opt => opt.MapFrom(src => src.StartTime.ToString(@"hh\:mm")))
               .ForMember(dest => dest.EndTimeStr, opt => opt.MapFrom(src => src.EndTime.ToString(@"hh\:mm")))
               .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.ClassLevel + src.Class.Section : string.Empty))
               .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.Lesson != null ? src.Lesson.Name : string.Empty))
               .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Name : string.Empty));

            CreateMap<LessonScheduleRequestModel, LessonSchedule>()
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Teacher, opt => opt.Ignore());

            CreateMap<LessonScheduleViewModel, LessonScheduleRequestModel>().ReverseMap();

            // Attendance Mappings
            CreateMap<Attendance, AttendanceViewModel>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.Name : string.Empty))
                .ForMember(dest => dest.StudentNumber, opt => opt.MapFrom(src => src.Student != null ? src.Student.StudentNumber.ToString() : string.Empty));

            CreateMap<Student, AttendanceViewModel>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.StudentNumber, opt => opt.MapFrom(src => src.StudentNumber.ToString()));

            CreateMap<AttendanceRequestModel, Attendance>()
                 .ForMember(dest => dest.Student, opt => opt.Ignore())
                 .ForMember(dest => dest.Lesson, opt => opt.Ignore());

            CreateMap<AttendanceViewModel, AttendanceRequestModel>().ReverseMap();
        }
    }
}
