using NoteVault.DAL.Entities;

namespace NoteVault.BLL.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
