using FluentValidation;
using NoteVault.BLL.DTOs.Auth;

namespace NoteVault.BLL.Validators
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        private const int MaxEmailLength = 256;
        private const int MinPasswordLength = 8;
        private const int MaxPasswordLength = 100;
        
        public UserLoginDtoValidator()
        {
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
