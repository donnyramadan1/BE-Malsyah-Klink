using APIKlinik.Application.Interfaces;
using APIKlinik.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace APIKlinik.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
