using Mapster;
using NoteVault.BLL.Common;
using NoteVault.BLL.DTOs.Pagination;
using NoteVault.BLL.DTOs.Tags;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Entities;
using NoteVault.DAL.Interfaces;

namespace NoteVault.BLL.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Result<PagedResponse<TagDto>>> GetPageAsync(Guid userId, PaginationRequest request, CancellationToken cancellationToken = default)
        {
            var (tags, totalCount) = await _tagRepository.GetPageAsync(userId, request.PageNumber, request.PageSize, cancellationToken);
            var dtos = tags.Adapt<IReadOnlyCollection<TagDto>>();

            var response = new PagedResponse<TagDto>(dtos, request.PageNumber, request.PageSize, totalCount);
            return Result<PagedResponse<TagDto>>.Success(response);
        }

        public async Task<Result<TagDto>> CreateAsync(Guid userId, TagCreateDto request, CancellationToken cancellationToken = default)
        {
            var tag = request.Adapt<Tag>();
            tag.Id = Guid.NewGuid();
            tag.UserId = userId;

            var createdTag = await _tagRepository.AddAsync(tag, cancellationToken);
            var dto = createdTag.Adapt<TagDto>();

            return Result<TagDto>.Success(dto);
        }

        public async Task<Result> DeleteAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
        {
            var deleted = await _tagRepository.DeleteAsync(userId, id, cancellationToken);

            if(!deleted)
            {
                return Result.Failure(ErrorCode.NotFound);
            }

            return Result.Success();
        }
    }
}
