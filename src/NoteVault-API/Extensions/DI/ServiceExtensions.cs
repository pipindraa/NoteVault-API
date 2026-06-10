using NoteVault.BLL.Interfaces;
using NoteVault.BLL.Services;
using NoteVault.DAL.Interfaces;
using NoteVault.DAL.Repositories;

namespace NoteVault_API.Extensions.DI
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<INoteService, NoteService>();

            return services;
        }
    }
}
