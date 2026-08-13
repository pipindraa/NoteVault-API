namespace NoteVault.BLL.DTOs.Auth
{
    public class UserLoginResponseDto
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
