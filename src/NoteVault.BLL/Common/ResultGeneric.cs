namespace NoteVault.BLL.Common
{
    public class Result<T> : Result
    {
        public T? Value { get;  }

        private Result(T? value, bool isSuccess, ErrorCode? error, string? errorMessage) : base(isSuccess, error, errorMessage)
        {
            Value = value;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, true, null, null);
        }
        public static Result<T> Failure(ErrorCode error, string? message = null)
        {
            return new Result<T> (default, false, error, message);
        }
    }
}
