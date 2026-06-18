using System.ComponentModel;
using System.Security;

namespace NoteVault.BLL.Common
{
    public class Result
    {
        private static readonly Result _success = new(true, Array.Empty<string>());

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IReadOnlyCollection<string> Errors { get; }

        public string? ErrorMessage => Errors.FirstOrDefault();

        protected Result(bool isSuccess, IReadOnlyCollection<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success() => _success;
        public static Result Failure(string errorMessage) => new(false, new[] {errorMessage});
        public static Result Failure(IReadOnlyCollection<string> errors) => new(false, errors);
    }
}
