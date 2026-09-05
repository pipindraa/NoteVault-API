using FluentValidation;
using NoteVault.DAL.Interfaces;

namespace NoteVault.BLL.Validators.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, IReadOnlyCollection<Guid>> MustBeValidTags<T>( this IRuleBuilder<T, IReadOnlyCollection<Guid>> ruleBuilder, ITagRepository tagRepository)
        {
            return ruleBuilder
                .MustAsync(async (tagIds, cancellationToken) =>
                {
                    if (tagIds is null || tagIds.Count == 0)
                    {
                        return true;
                    }

                    var tags = await tagRepository.GetByIdsAsync(Guid.Empty, tagIds, cancellationToken);
                    return tags.Count == tagIds.Distinct().Count();
                })
                .WithErrorCode("InvalidTagsIds");
        }
    }
}
