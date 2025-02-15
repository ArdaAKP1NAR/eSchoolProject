using eSchoolDatabase;
using eSchoolDatabase.Models;
using eSchoolDatabase.Repositories.Interface;
using eSchoolProject.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace eSchoolProject.Components.Pages
{
    public partial class Login
    {
        [SupplyParameterFromForm] public LoginModel loginModel { get; set; } = new();
        [CascadingParameter] public HttpContext? httpContext { get; set; }
        [Inject] IServiceScopeFactory serviceScopeFactory { get; init; } = default!;
        [Inject] NavigationManager navigationManager { get; set; } = default!;

        private async Task HandleLoginAsync()
        {
            using var scope = serviceScopeFactory.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var user = await userRepository.GetAll()
                .FirstOrDefaultAsync(a => a.IdentityNumber == loginModel.IdentityNumber);

            string inputPassword = loginModel.Password;
            bool isMatch = BCrypt.Net.BCrypt.Verify(inputPassword, user.Password);
            if (isMatch)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, loginModel.IdentityNumber)
                };
                foreach (var item in user.Roles)
                {
                    claims.Add(new(ClaimTypes.Role, Enum.GetName(item)));
                }
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await httpContext.SignInAsync(principal);
                navigationManager.NavigateTo("/");
            }
        }
        public class LoginModel
        {
            [MaxLength(100)]
            public string IdentityNumber { get; set; } = default!;
            [MaxLength(100)]
            public string Password { get; set; } = default!;
        }
    }
}