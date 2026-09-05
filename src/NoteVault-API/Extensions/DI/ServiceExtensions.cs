using FluentValidation;
using FluentValidation.AspNetCore;
using NoteVault.BLL.Interfaces;
using NoteVault.BLL.Services;
using NoteVault.BLL.Validators;
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
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagService, TagService>();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(NoteCreateDtoValidator).Assembly);

            return services;
        }
    }
}
