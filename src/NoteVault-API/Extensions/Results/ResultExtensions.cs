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
            var problem = new ProblemDetails
            {
                Detail = result.ErrorMessage ?? "An error occurred."
            };

            return result.Error switch
            {
                ErrorCode.NotFound => new NotFoundObjectResult(problem),
                ErrorCode.ValidationError => new BadRequestObjectResult(problem),
                ErrorCode.EmailAlreadyExists => new ConflictObjectResult(problem),
                ErrorCode.UsernameAlreadyExists => new ConflictObjectResult(problem),
                ErrorCode.InvalidCredentials => new BadRequestObjectResult(problem),
                _ => new ObjectResult(problem) { StatusCode = StatusCodes.Status500InternalServerError}
            };
        }
    }
}
