namespace NoteVault.BLL.Common
{
    public class Result
    {
        private static readonly Result _success = new(true, null);

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? ErrorMessage { get; }

        protected Result(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => _success;
        public static Result Failure(string errorMessage) => new(false, errorMessage);
    }
}
