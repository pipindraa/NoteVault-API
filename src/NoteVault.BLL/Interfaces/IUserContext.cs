namespace NoteVault.BLL.Interfaces
{
    public interface IUserContext
    {
        bool TryGetUserId(out Guid userId);
    }
}
