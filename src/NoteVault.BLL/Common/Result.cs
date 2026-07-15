namespace NoteVault.BLL.Common
{
    public class Result
    {
        private static readonly Result _success = new(true, null, null);

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public ErrorCode? Error { get; }
        public string? ErrorMessage { get; }

        protected Result(bool isSuccess, ErrorCode? error, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Error = error;
            ErrorMessage = errorMessage;
        }

        public static Result Success()
        {  
            return _success; 
        }
        public static Result Failure(ErrorCode error, string? message = null)
        {
            return new Result(false, error, message);
        }
    }
}
