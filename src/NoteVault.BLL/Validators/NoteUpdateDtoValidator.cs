using FluentValidation;
using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Validators.Extensions;

namespace NoteVault.BLL.Validators
{
    public class NoteUpdateDtoValidator : AbstractValidator<NoteUpdateDto>
    {
        private const int MaxNameLength = 100;
        private const int MaxDescriptionLength = 1000;

        public NoteUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(MaxNameLength);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(MaxDescriptionLength);

            RuleFor(x => x.TagIds)
                .MustHaveUniqueElements();

            RuleForEach(x => x.TagIds)
                .NotEmpty()
                .WithErrorCode("EmptyTagId");
        }
    }
}
