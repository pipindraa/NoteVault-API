namespace NoteVault.BLL.Common
{
    public class Result<T> : Result
    {
        private readonly T? _value;

        public T Value
        {
            get
            {
                return _value!;
            }
        }

        private Result(T? value, bool isSuccess, IReadOnlyCollection<Error> errors) : base(isSuccess, errors)
        {
            _value = value;
        }

        private Result(bool isSuccess, IReadOnlyCollection<Error> errors) : base(isSuccess, errors)
        {
            _value = default;
        }

        public static Result<T> Success(T value) => new(value, true, Array.Empty<Error>());
        public static Result<T> Failure(Error error) => new(false, new[] { error });
        public static Result<T> Failure(IReadOnlyCollection<Error> errors) => new(false, errors);
    }
}
