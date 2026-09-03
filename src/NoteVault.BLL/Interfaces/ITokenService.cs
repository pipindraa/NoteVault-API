using NoteVault.DAL.Entities;

namespace NoteVault.BLL.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
    }
}
