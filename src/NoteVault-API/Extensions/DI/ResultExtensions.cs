using Microsoft.AspNetCore.Mvc;
using NoteVault.BLL.Common;
using NoteVault_API.Models;
using System.Reflection.Metadata.Ecma335;

namespace NoteVault_API.Extensions.DI
{
    public static class ResultExtensions
    {
        public static ActionResult ToActionResult(this Result result)
        {
            if (result.IsSuccess)
            {
                return new NoContentResult();
            }

            return ToErrorActionResult(result);
        }

        public static ActionResult<T> ToActionResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }

            return (ActionResult<T>)ToErrorActionResult(result);
        }

        private static ActionResult ToErrorActionResult(this Result result)
        {
            var primaryError = result.Errors.FirstOrDefault();
            int statusCode = GetHttpStatusCode(primaryError);

            var errorResponse = new ErrorResponse
            {
                StatusCode = statusCode,
                Errors = result.Errors.ToList()
            };

            return statusCode switch
            {
                StatusCodes.Status404NotFound => new NotFoundObjectResult(errorResponse),
                StatusCodes.Status400BadRequest => new BadRequestObjectResult(errorResponse),
                _ => new OkObjectResult(errorResponse) { StatusCode = statusCode }
            };
        }

        private static int GetHttpStatusCode(ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.NotFound => StatusCodes.Status404NotFound,
                ErrorCode.ValidationError => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status400BadRequest
            };
        }
    }
}
