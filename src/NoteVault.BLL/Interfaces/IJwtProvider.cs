using NoteVault.BLL.DTOs.Auth;

namespace NoteVault.BLL.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(AuthUserDto user);
    }
}
