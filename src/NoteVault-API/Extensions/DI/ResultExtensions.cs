using Microsoft.AspNetCore.Mvc;
using NoteVault.BLL.Common;
using NoteVault_API.Models;

namespace NoteVault_API.Extensions.DI
{
    public static class ResultExtensions
    {
        public static ActionResult ToActionResult(this Result result, ControllerBase controller)
        {
            if (result.IsSuccess)
            {
                return controller.NoContent();
            }

            return ToErrorActionResult(result, controller);
        }

        public static ActionResult<T> ToActionResult<T>(this Result<T> result, ControllerBase controller)
        {
            if (result.IsSuccess)
            {
                return controller.Ok(result.Value);
            }

            return (ActionResult<T>)ToErrorActionResult(result, controller);
        }

        private static ActionResult ToErrorActionResult(this Result result, ControllerBase controller)
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
                StatusCodes.Status404NotFound => controller.NotFound(errorResponse),
                StatusCodes.Status400BadRequest => controller.BadRequest(errorResponse),
                _ => controller.StatusCode(statusCode, errorResponse)
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
