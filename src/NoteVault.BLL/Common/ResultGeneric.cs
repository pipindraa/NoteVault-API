namespace NoteVault.BLL.Common
{
    public class Result<T> : Result
    {
        public T Value { get;  }

        private Result(T? value, bool isSuccess, IReadOnlyCollection<ErrorCode> errors, IEnumerable<string> errorMessages) : base(isSuccess, errors, errorMessages)
        {
            Value = value;
        }

        private Result(bool isSuccess, IReadOnlyCollection<ErrorCode> errors, IEnumerable<string> errorMessages) : base(isSuccess, errors,  errorMessages)
        {
            Value = default;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, true, Array.Empty<ErrorCode>(), Enumerable.Empty<string>());
        }
        public static Result<T> Failure(ErrorCode error, IEnumerable<string>? messages = null)
        {
            return new Result<T> (false, new[] { error }, messages ?? Enumerable.Empty<string>());
        }
            
        public static Result<T> Failure(IReadOnlyCollection<ErrorCode> errors, IEnumerable<string>? messages = null)
        {
            return new Result<T>(false, errors, messages ?? Enumerable.Empty<string>());
        }
    }
}
