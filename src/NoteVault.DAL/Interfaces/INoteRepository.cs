using NoteVault.DAL.Entities;
using System.Linq.Expressions;

namespace NoteVault.DAL.Interfaces
{
    public interface INoteRepository
    {
        Task<(List<Note> Items, int TotalCount)> GetPageAsync<TKey>(Expression<Func<Note, TKey>> orderBy, int pageNumber, int pageSize, bool descending = true, CancellationToken cancellationToken = default);
        Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Note> AddAsync(Note note, CancellationToken cancellationToken = default);
        Task UpdateAsync(Note note, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
