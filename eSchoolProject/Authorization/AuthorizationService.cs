using AutoMapper;
using eSchoolDatabase.Repositories.Interface;
using eSchoolProject.Authorization.Interface;
using System.Security.Claims;
using System.Security.Principal;

namespace eSchoolProject.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private ClaimsPrincipal? User;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AuthorizationService(IUserRepository userRepository, IMapper mapper, IPrincipal principal)
        {
            User = principal as ClaimsPrincipal;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public string GetCurrentLoggedInUserName()
        {
            return User.Claims.Single(a => a.Type == ClaimTypes.Name).Value;
        }
        public bool UserHasRole(string role)
        {
            return User.Claims.Any(a => a.Type == ClaimTypes.Role && a.Value == role);
        }
        public bool IsLoggedIn()
        {
            return User!.Claims.Any(a => a.Type == ClaimTypes.Role);
        }
    }
}
