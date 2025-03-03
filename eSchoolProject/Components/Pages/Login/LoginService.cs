using AutoMapper;
using BCrypt.Net;
using eSchool.Utils;
using eSchoolDatabase;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories;
using eSchoolDatabase.Repositories.Interface;
using eSchoolDatabase.RequestModels;

namespace eSchoolProject.Components.Pages.Login
{
    public class LoginService(IUserRepository userRepository, IMapper mapper) : ILoginService
    {
        public async Task<User> AddUserAsync(UserRequestModel userRequestModel,CancellationToken cancellationToken)
        {
            var obj = mapper.Map<User>(userRequestModel);
            obj.Password = BCrypt.Net.BCrypt.HashPassword(userRequestModel.Password);
            return await userRepository.AddAsync(obj,cancellationToken);
        }

    }
}
