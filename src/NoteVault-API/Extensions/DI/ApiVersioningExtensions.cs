using Asp.Versioning;
using NoteVault_API.Constants;

namespace NoteVault_API.Extensions.DI
{
    public static class ApiVersioningExtensions
    {
        public static IServiceCollection AddApiVersioningSupport(this IServiceCollection services)
        {
            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(ApiVersions.MajorVersion, ApiVersions.MinorVersion);
                    options.AssumeDefaultVersionWhenUnspecified = ApiVersions.AssumeDefaultVersion;
                    options.ReportApiVersions = ApiVersions.ReportApiVersions;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = ApiVersions.GroupNameFormat;
                    options.SubstituteApiVersionInUrl = ApiVersions.SubstiteApiVersionInUrl;
                });

            return services;
        }
    }
}
