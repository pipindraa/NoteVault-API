using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace NoteVault_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                var statusCode = HttpStatusCode.InternalServerError;

                await HandleExceptionAsync(context, statusCode);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode)
        {
            context.Response.StatusCode = (int)statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
