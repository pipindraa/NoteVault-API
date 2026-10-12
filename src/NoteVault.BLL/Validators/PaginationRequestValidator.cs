using FluentValidation;
using NoteVault.BLL.DTOs.Pagination;

namespace NoteVault.BLL.Validators
{
    public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
    {
        private const int MinPageNumber = 1;
        private const int MinPageSize = 1;
        private const int MaxPageSize = 100;

        public PaginationRequestValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(MinPageNumber);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(MinPageSize, MaxPageSize);
        }
    }
}
