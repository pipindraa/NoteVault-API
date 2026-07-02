namespace NoteVault.BLL.Common
{
    public class Result
    {
        private static readonly Result _success = new(true, Array.Empty<ErrorCode>(), Enumerable.Empty<string>());

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IReadOnlyCollection<ErrorCode> Errors { get; }
        public IEnumerable<string> ErrorMessages { get; }

        protected Result(bool isSuccess, IReadOnlyCollection<ErrorCode> errors, IEnumerable<string> errorMessages)
        {
            IsSuccess = isSuccess;
            Errors = errors;
            ErrorMessages = errorMessages;
        }

        public static Result Success()
        {  
            return _success; 
        }
        public static Result Failure(ErrorCode error, IEnumerable<string>? messages = null)
        {
            return new Result(false, new[] { error }, messages ?? Enumerable.Empty<string>());
        }
        public static Result Failure(IReadOnlyCollection<ErrorCode> errors, IEnumerable<string>? messages = null)
        {
            return new Result(false, errors, messages ?? Enumerable.Empty<string>());
        }
    }
}
