using NoteVault.BLL.Exceptions;
using System.Net;
using System.Text.Json;
using NoteVault_API.Models;
using NoteVault_API.Constants;

namespace NoteVault_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private static readonly Dictionary<Type, HttpStatusCode> ExceptionStatusCodes = new()
        {
            { typeof(NotFoundException), HttpStatusCode.NotFound  },
        };

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
                var isKnownException = ExceptionStatusCodes.TryGetValue(ex.GetType(), out var statusCode);
                if (!isKnownException)
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }

                if (isKnownException)
                {
                    _logger.LogWarning(ex, ex.Message);
                }
                else
                {
                    _logger.LogError(ex, "An unexpected error occurred.");
                }
                    
                var message = isKnownException ? ex.Message : ErrorMessages.UnexpectedError;
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

            context.Response.ContentType = ContentTypes.Json;
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
