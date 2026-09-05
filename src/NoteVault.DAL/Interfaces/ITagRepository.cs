using NoteVault.DAL.Entities;

namespace NoteVault.DAL.Interfaces
{
    public interface ITagRepository
    {
        Task<(List<Tag> Items, int TotalCount)> GetPageAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<Tag?> GetByIdAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
        Task<List<Tag>> GetByIdsAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        Task<Tag> AddAsync(Tag tag, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default);
    }
}
