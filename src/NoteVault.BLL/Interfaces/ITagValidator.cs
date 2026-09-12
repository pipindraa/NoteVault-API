using NoteVault.BLL.Common;
using NoteVault.DAL.Entities;

namespace NoteVault.BLL.Interfaces
{
    public interface ITagValidator
    {
        Task<Result<List<Tag>>> EnsureAllExistAsync(Guid userId, IEnumerable<Guid>? tagIds, CancellationToken cancellationToken = default);
    }
}
