using AutoMapper;
using eSchoolDatabase.Models;
using eSchoolDatabase.RequestModel;
using eSchoolDatabase.RequestModels;
using eSchoolDatabase.ViewModels;

namespace eSchoolProject.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<School, SchoolRequestModel>().ReverseMap();
            CreateMap<School, SchoolViewModel>().ReverseMap();
            CreateMap<SchoolRequestModel, SchoolViewModel>().ReverseMap();

            CreateMap<Address, AddressRequestModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
            CreateMap<AddressRequestModel, AddressViewModel>().ReverseMap();

            CreateMap<Manager, ManagerRequestModel>().ReverseMap();
            CreateMap<Manager, ManagerViewModel>().ReverseMap();

            CreateMap<Teacher, TeacherRequestModel>().ReverseMap();
            CreateMap<Teacher, TeacherViewModel>().ReverseMap();

            CreateMap<Student, StudentRequestModel>().ReverseMap();
            CreateMap<Student, StudentViewModel>().ReverseMap();

            CreateMap<Class, ClassViewModel>().ReverseMap();
            CreateMap<Class, ClassRequestModel>().ReverseMap();

            CreateMap<Lesson, LessonViewModel>().ReverseMap();

            CreateMap<Grade, GradeViewModel>().ReverseMap();
        }
    }
}
