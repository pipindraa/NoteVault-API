using NoteVault.BLL.Common;
using NoteVault.BLL.DTOs.Pagination;
using NoteVault.BLL.DTOs.Tags;

namespace NoteVault.BLL.Interfaces
{
    public interface ITagService
    {
        Task<Result<IReadOnlyCollection<TagDto>>> GetPageAsync(PaginationRequest request, CancellationToken cancellationToken = default);
        Task<Result<TagDto>> CreateAsync(TagCreateDto request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(Guid id, CancellationToken cancellation = default);
    }
}
