using FluentValidation;
using NoteVault.BLL.Interfaces;
using NoteVault.DAL.Interfaces;

namespace NoteVault.BLL.Validators.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, IReadOnlyCollection<Guid>?> MustHaveUniqueElements<T>(this IRuleBuilder<T, IReadOnlyCollection<Guid>?> ruleBuilder)
        {
            return ruleBuilder
                .Must(collection => collection == null || collection.Distinct().Count() == collection.Count())
                .WithErrorCode("DuplicateTagIds");
        }

        public static IRuleBuilderOptions<T, IReadOnlyCollection<Guid>?> MustBelongToCurrentUser<T>(this IRuleBuilder<T, IReadOnlyCollection<Guid>?> ruleBuilder, ITagRepository tagRepository, IUserContext userContext)
        {
            return ruleBuilder
                .MustAsync(async (tagIds, cancellationToken) =>
                {
                    if (tagIds is null || tagIds.Count == 0)
                    {
                        return true;
                    }

                    if (!userContext.TryGetUserId(out var userId))
                    {
                        return false;
                    }

                    var distinctTagIds = tagIds.Distinct().ToList();
                    var tags = await tagRepository.GetByIdsAsync(userId, distinctTagIds, cancellationToken);
                    return tags.Count == distinctTagIds.Count;
                })
                .WithErrorCode("TagNotFound");
        }
    }
}
