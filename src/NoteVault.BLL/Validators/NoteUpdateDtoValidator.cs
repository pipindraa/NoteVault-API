using FluentValidation;
using NoteVault.BLL.DTOs.Notes;
using NoteVault.DAL.Interfaces;
using NoteVault.BLL.Validators.Extensions;

namespace NoteVault.BLL.Validators
{
    public class NoteUpdateDtoValidator : AbstractValidator<NoteUpdateDto>
    {
        private readonly ITagRepository _tagRepository;

        private const int MaxNameLength = 100;
        private const int MaxDescriptionLength = 1000;

        public NoteUpdateDtoValidator(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(MaxNameLength);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(MaxDescriptionLength);

            RuleFor(x => x.TagIds)
                .MustBeValidTags(tagRepository);
        }
    }
}
