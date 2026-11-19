using FluentValidation;
using NoteVault.BLL.DTOs.Tags;

namespace NoteVault.BLL.Validators
{
    public class TagCreateDtoValidator : AbstractValidator<TagCreateDto>
    {
        private const int MaxNameLength = 50;

        public TagCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(MaxNameLength);
        }
    }
}
