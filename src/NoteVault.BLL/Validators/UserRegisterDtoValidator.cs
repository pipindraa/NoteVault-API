using FluentValidation;
using NoteVault.BLL.DTOs.Auth;

namespace NoteVault.BLL.Validators
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        private const int MaxUsernameLenght = 50;
        private const int MaxEmailLength = 256;
        private const int MinPasswordLength = 8;
        private const int MaxPasswordLength = 100;

        public UserRegisterDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MaximumLength(MaxUsernameLenght);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(MaxEmailLength);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(MinPasswordLength)
                .MaximumLength(MaxPasswordLength);
        }
    }
}
