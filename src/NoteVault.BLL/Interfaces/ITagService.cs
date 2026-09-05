using NoteVault.BLL.Common;
using NoteVault.BLL.DTOs.Pagination;
using NoteVault.BLL.DTOs.Tags;

namespace NoteVault.BLL.Interfaces
{
    public interface ITagService
    {
        Task<Result<PagedResponse<TagDto>>> GetPageAsync(Guid userId, PaginationRequest request, CancellationToken cancellationToken = default);
        Task<Result<TagDto>> CreateAsync(Guid userId, TagCreateDto request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(Guid userId, Guid id, CancellationToken cancellation = default);
    }
}
