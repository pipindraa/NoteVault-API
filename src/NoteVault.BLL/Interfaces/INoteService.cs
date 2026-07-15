using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Common;
using NoteVault.BLL.DTOs.Pagination;

namespace NoteVault.BLL.Interfaces
{
    public interface INoteService
    {
        Task<Result<IReadOnlyCollection<NoteResponseDto>>> GetAllAsync(PaginationRequest request, CancellationToken cancellationToken = default);
        Task<Result<NoteResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result<NoteResponseDto>> CreateAsync(NoteCreateDto request, CancellationToken cancellationToken = default);
        Task<Result<NoteResponseDto>> UpdateAsync(Guid id, NoteUpdateDto request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}