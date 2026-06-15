namespace NoteVault.BLL.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? ErrorMessage { get; }

        protected Result(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(string errorMessage) => new(false, errorMessage);
    }

    public class Result<T> : Result
    {
        private readonly T? _value;

        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access the value of a failed result.");

        private Result(T? value, bool isSuccess, string? errorMessage) : base(isSuccess, errorMessage)
        {
            _value = value;
        }

        public static Result<T> Success(T value) => new(value, true, null);
        public static new Result<T> Failure(string errorMessage) => new(default, false, errorMessage);
    }
}
