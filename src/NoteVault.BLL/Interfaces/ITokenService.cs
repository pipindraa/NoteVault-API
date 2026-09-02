using NoteVault.DAL.Entities;

namespace NoteVault.BLL.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
