using eSchoolDatabase;
using System.ComponentModel.DataAnnotations;

namespace eSchoolProject.Components.Pages.Login
{
    public class UserRequestModel
    {
        public string IdentityNumber { get; set; } = default!;
        public string Password { get; set; } = default!;
        public List<Roles> Roles { get; set; } = new();
    }
}