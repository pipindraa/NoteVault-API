using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Common;
using NoteVault.BLL.DTOs.Pagination;

namespace NoteVault.BLL.Interfaces
{
    public interface INoteService
    {
        Task<Result<IReadOnlyCollection<NoteResponseDto>>> GetAllAsync(Guid userId, PaginationRequest request, CancellationToken cancellationToken = default);
        Task<Result<NoteResponseDto>> GetByIdAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default);
        Task<Result<NoteResponseDto>> CreateAsync(Guid userId, NoteCreateDto request, CancellationToken cancellationToken = default);
        Task<Result<NoteResponseDto>> UpdateAsync(Guid userId, Guid noteId, NoteUpdateDto request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(Guid userId, Guid noteId, CancellationToken cancellationToken = default);
    }
}