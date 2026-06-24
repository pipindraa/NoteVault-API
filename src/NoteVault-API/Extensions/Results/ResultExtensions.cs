using Microsoft.AspNetCore.Mvc;
using NoteVault.BLL.Common;

namespace NoteVault_API.Extensions.Results
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
            var statusCode = GetHttpStatusCode(result.Errors.FirstOrDefault());

            return statusCode switch
            {
                StatusCodes.Status404NotFound => new NotFoundObjectResult(result.Errors),
                StatusCodes.Status400BadRequest => new BadRequestObjectResult(result.Errors),
                _ => new StatusCodeResult(statusCode)
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
