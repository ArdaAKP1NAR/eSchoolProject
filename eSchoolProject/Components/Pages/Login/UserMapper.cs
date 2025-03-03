using AutoMapper;
using eSchoolDatabase.Models;

namespace eSchoolProject.Components.Pages.Login
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserRequestModel, User>();
        }
    }
}
