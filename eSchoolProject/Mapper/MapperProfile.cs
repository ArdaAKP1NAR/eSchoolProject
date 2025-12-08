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
                .ForMember(dest => dest.TeacherId,
                           opt => opt.MapFrom(src => src.Lesson.TeacherId));
            CreateMap<LessonScheduleRequestModel, LessonSchedule>()
                .ForMember(dest => dest.Lesson, opt => opt.Ignore())
                .ForMember(dest => dest.Teacher, opt => opt.Ignore());
            CreateMap<LessonScheduleViewModel, LessonScheduleRequestModel>().ReverseMap();
        }
    }
}
