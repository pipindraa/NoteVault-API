using NoteVault_API.Middleware;

namespace NoteVault_API.Extensions.DI
{
    public static class MiddlewareExtensions
    {
        public static WebApplication UseGlobalExceptionHandling(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }
}
