using System.ComponentModel;
using System.Security;

namespace NoteVault.BLL.Common
{
    public class Result
    {
        private static readonly Result _success = new(true, Array.Empty<Error>());

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IReadOnlyCollection<Error> Errors { get; }

        public string? ErrorMessage => Errors.FirstOrDefault()?.Message;

        protected Result(bool isSuccess, IReadOnlyCollection<Error> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success() => _success;
        public static Result Failure(Error error) => new(false, new[] {error});
        public static Result Failure(IReadOnlyCollection<Error> errors) => new(false, errors);
    }
}
