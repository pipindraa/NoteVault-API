namespace NoteVault.BLL.DTOs.Auth
{
    public class UserRegisterResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public static UserRegisterResponseDto Create(string token) => new() { Token = token };
    }
}
