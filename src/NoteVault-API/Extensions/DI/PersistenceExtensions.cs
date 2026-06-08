using Microsoft.EntityFrameworkCore;
using NoteVault.DAL.Data;
using NoteVault_API.Constants;

namespace NoteVault_API.Extensions.DI
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString(ConnectionStrings.DefaultConnection)));

            return services;
        }
    }
}
