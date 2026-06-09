using NoteVault.BLL.DTOs.Notes;

namespace NoteVault.BLL.Interfaces
{
    public interface INoteService
    {
        Task<IReadOnlyCollection<NoteResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<NoteResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<NoteResponseDto> CreateAsync(NoteCreateDto request, CancellationToken cancellationToken = default);
        Task<NoteResponseDto?> UpdateAsync(Guid id, NoteUpdateDto request, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}