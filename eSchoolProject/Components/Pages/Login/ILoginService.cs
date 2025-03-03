using eSchoolDatabase.Models;

namespace eSchoolProject.Components.Pages.Login
{
    public interface ILoginService
    {
        Task<User> AddUserAsync(UserRequestModel userRequestModel,CancellationToken cancellationToken);
    }
}