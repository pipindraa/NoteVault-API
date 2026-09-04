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

        public async Task<Result<IReadOnlyCollection<TagDto>>> GetPageAsync(PaginationRequest request, CancellationToken cancellationToken = default)
        {
            var tags = await _tagRepository.GetPageAsync(request.PageNumber, request.PageSize, cancellationToken);
            var dtos = tags.Adapt<IReadOnlyCollection<TagDto>>();

            return Result<IReadOnlyCollection<TagDto>>.Success(dtos);
        }

        public async Task<Result<TagDto>> CreateAsync(TagCreateDto request, CancellationToken cancellationToken = default)
        {
            var tag = request.Adapt<Tag>();
            tag.Id = Guid.NewGuid();

            var createdTag = await _tagRepository.AddAsync(tag, cancellationToken);
            var dto = createdTag.Adapt<TagDto>();

            return Result<TagDto>.Success(dto);
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var deleted = await _tagRepository.DeleteAsync(id, cancellationToken);

            if(!deleted)
            {
                return Result.Failure(ErrorCode.NotFound);
            }

            return Result.Success();
        }
    }
}
