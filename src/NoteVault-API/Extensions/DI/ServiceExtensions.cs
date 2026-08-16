using NoteVault.BLL.Common;
using NoteVault.BLL.Interfaces;
using NoteVault.BLL.Services;
using NoteVault.DAL.Interfaces;
using NoteVault.DAL.Repositories;

namespace NoteVault_API.Extensions.DI
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PasswordHashingOptions>(configuration.GetSection(nameof(PasswordHashingOptions)));

            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
