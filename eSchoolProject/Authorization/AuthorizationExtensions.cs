using eSchoolDatabase;
using eSchoolProject.Authorization.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace eSchoolProject.Authorization
{
    public static class AuthorizationExtensions
    {
        public static void ConfigureAuthorizationServices(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "auth_token";
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/acces-denied";
                options.Cookie.MaxAge = TimeSpan.FromDays(1);
            });
            services.AddAuthorization(options =>
            {
                foreach (var item in Enum.GetNames(typeof(Roles)))
                {
                    options.AddPolicy(item, policy => policy.RequireRole(item));
                }
            });

            services.AddCascadingAuthenticationState();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IPrincipal>(
                (sp) => sp.GetService<IHttpContextAccessor>()!.HttpContext!.User);
        }
        public static void ConfigureAuthorizationMiddleWare(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
        }
    }
}
