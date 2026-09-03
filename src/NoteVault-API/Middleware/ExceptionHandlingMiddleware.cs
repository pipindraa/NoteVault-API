using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace NoteVault_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private const string UnexpectedErrorMessage = "An unexpected error occurred.";

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

                await HandleExceptionAsync(context, statusCode, UnexpectedErrorMessage);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Detail = message
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
