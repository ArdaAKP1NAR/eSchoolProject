namespace eSchoolProject.Authorization.Interface
{
    public interface IAuthorizationService
    {
        string GetCurrentLoggedInUserName();
        bool IsLoggedIn();
        bool UserHasRole(string role);
    }
}