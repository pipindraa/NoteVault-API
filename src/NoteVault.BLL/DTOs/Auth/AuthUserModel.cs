namespace NoteVault.BLL.DTOs.Auth
{
    public class AuthUserModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
