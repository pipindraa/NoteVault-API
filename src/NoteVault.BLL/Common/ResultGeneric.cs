namespace NoteVault.BLL.Common
{
    public class Result<T> : Result
    {
        public T Value { get;  }

        private Result(T? value, bool isSuccess, IReadOnlyCollection<ErrorCode> errors) : base(isSuccess, errors)
        {
            Value = value;
        }

        private Result(bool isSuccess, IReadOnlyCollection<ErrorCode> errors) : base(isSuccess, errors)
        {
            Value = default;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, true, Array.Empty<ErrorCode>());
        }
        public static Result<T> Failure(ErrorCode error)
        {
            return new Result<T> (false, new[] { error });
        }
            
        public static Result<T> Failure(IReadOnlyCollection<ErrorCode> errors)
        {
            return new Result<T>(false, errors);
        }
    }
}
