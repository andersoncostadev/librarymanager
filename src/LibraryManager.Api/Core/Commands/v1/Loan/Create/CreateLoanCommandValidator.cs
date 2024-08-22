using FluentValidation;

namespace Core.Commands.v1.Loan.Create
{
    public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
    {
        public CreateLoanCommandValidator()
        {
            RuleFor(x => x.BookId)
                .NotEmpty()
                .Must(BeAValidGuid).WithMessage("Invalid GUID format.");


            RuleFor(x => x.UserId)
                .NotEmpty()
                .Must(BeAValidGuid).WithMessage("Invalid GUID format.");


            RuleFor(x => x.LoanDate)
                .NotEmpty()
                .WithMessage("The Loan Date is require.");

            RuleFor(x => x.ReturnDate)
                .NotEmpty()
                .WithMessage("The Return Date is require.");
        }

        private bool BeAValidGuid(Guid id)
        {
            return Guid.TryParse(id.ToString(), out _);
        }
    }
}
