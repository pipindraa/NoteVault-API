using FluentValidation;
using NoteVault.BLL.DTOs.Notes;
using NoteVault.BLL.Interfaces;
using NoteVault.BLL.Validators.Extensions;
using NoteVault.DAL.Interfaces;

namespace NoteVault.BLL.Validators
{
    public class NoteCreateDtoValidator : AbstractValidator<NoteCreateDto>
    {
        private const int MaxNameLength = 100;
        private const int MaxDescriptionLength = 1000;

        public NoteCreateDtoValidator(ITagRepository tagRepository, IUserContext userContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(MaxNameLength);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(MaxDescriptionLength);

            RuleFor(x => x.TagIds)
                .MustHaveUniqueElements()
                .MustBelongToCurrentUser(tagRepository, userContext);

            RuleForEach(x => x.TagIds)
                .NotEmpty()
                .WithErrorCode("EmptyTagId");
        }
    }
}
