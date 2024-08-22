using FluentValidation;

namespace Core.Commands.v1.Book.Delete
{
    public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookCommandValidator()
        {
            RuleFor(x => x.Id)
                 .NotEmpty()
                 .Must(BeAValidGuid).WithMessage("Invalid GUID format.");
        }

        private bool BeAValidGuid(Guid id)
        {
            return Guid.TryParse(id.ToString(), out _);
        }
    }
}
