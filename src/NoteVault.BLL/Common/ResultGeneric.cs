namespace NoteVault.BLL.Common
{
    public class Result<T> : Result
    {
        private readonly T? _value;

        public T Value
        {
            get
            {
                if (IsSuccess)
                {
                    return _value!;
                }

                throw new InvalidOperationException("Cannot access the value of a failed result.");
            }
        }

        private Result(T? value, bool isSuccess, IReadOnlyCollection<string> errors) : base(isSuccess, errors)
        {
            _value = value;
        }

        private Result(bool isSuccess, IReadOnlyCollection<string> errors) : base(isSuccess, errors)
        {
            _value = default;
        }

        public static Result<T> Success(T value) => new(value, true, Array.Empty<string>());
        public static Result<T> Failure(string errorMessage) => new(false, new[] { errorMessage });
        public static Result<T> Failure(IReadOnlyCollection<string> errors) => new(false, errors);
    }
}
