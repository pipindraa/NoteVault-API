using NoteVault.BLL.Common;

namespace NoteVault_API.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public IReadOnlyCollection<ErrorCode> Errors { get; set; } = Array.Empty<ErrorCode>();
    }
}
