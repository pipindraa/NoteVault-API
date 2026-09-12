using NoteVault.BLL.Common;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Entities;
using NoteVault.DAL.Interfaces;

namespace NoteVault.BLL.Services
{
    public class TagValidator : ITagValidator
    {
        private readonly ITagRepository _tagRepository;

        public TagValidator(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Result<List<Tag>>> EnsureAllExistAsync(Guid userId, IEnumerable<Guid>? tagIds, CancellationToken cancellationToken = default)
        {
            if (tagIds is not { } ids || !ids.Any())
            {
                return Result<List<Tag>>.Success(new List<Tag>());
            }

            var distinctTagIds = ids.Distinct().ToList();
            var tags = await _tagRepository.GetByIdsAsync(userId, distinctTagIds, cancellationToken);

            if (tags is not { Count: var count } || count != distinctTagIds.Count )
            {
                return Result<List<Tag>>.Failure(ErrorCode.TagNotFound);
            }

            return Result<List<Tag>>.Success(tags);
        }
    }
}
