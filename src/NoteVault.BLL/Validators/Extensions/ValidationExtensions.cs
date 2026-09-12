using FluentValidation;

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
    }
}
