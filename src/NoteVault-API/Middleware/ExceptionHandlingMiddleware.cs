using System.Net;
using NoteVault_API.Models;
using NoteVault_API.Constants;

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
                var (statusCode, message) = ex switch
                {
                    _ => (HttpStatusCode.InternalServerError, ErrorMessages.UnexpectedError)
                };
                if (statusCode == HttpStatusCode.InternalServerError)
                {
                    _logger.LogError(ex, ErrorMessages.UnexpectedError);
                }
                else
                {
                    _logger.LogWarning(ex, ex.Message);
                }
                await HandleExceptionAsync(context, statusCode, message);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            var response = new ErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = message
            };

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
