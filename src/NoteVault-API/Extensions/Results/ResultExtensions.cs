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
            return result.Error switch
            {
                ErrorCode.NotFound => new NotFoundObjectResult(result.ErrorMessage),
                ErrorCode.ValidationError => new BadRequestObjectResult(result.ErrorMessage),
                _ => new BadRequestObjectResult(result.ErrorMessage ?? "An error occurred.")
            };
        }
    }
}
