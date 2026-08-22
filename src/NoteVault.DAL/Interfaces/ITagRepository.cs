using NoteVault.DAL.Entities;

namespace NoteVault.DAL.Interfaces
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<Tag?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Tag>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        Task<Tag> AddAsync(Tag tag, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
