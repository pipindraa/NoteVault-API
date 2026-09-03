namespace NoteVault.BLL.DTOs.Auth
{
    public class UserLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public static UserLoginResponseDto Create(string token) => new() { Token = token };
    }
}
