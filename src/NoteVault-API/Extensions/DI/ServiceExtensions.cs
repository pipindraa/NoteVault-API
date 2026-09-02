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
            services.Configure<PasswordHashingOptions>(configuration.GetSection(PasswordHashingOptions.SectionName));

            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IJwtProvider, JwtProvider>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ITokenService, TokenService>();

            return services;
        }
    }
}
