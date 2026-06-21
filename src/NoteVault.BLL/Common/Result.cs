using System.Linq;

namespace NoteVault.BLL.Common
{
    public class Result
    {
        private static readonly Result _success = new(true, Array.Empty<ErrorCode>());

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IReadOnlyCollection<ErrorCode> Errors { get; }

        protected Result(bool isSuccess, IReadOnlyCollection<ErrorCode> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success() => _success;
        public static Result Failure(ErrorCode error) => new(false, new[] {error});
        public static Result Failure(IReadOnlyCollection<ErrorCode> errors) => new(false, errors);
    }
}
