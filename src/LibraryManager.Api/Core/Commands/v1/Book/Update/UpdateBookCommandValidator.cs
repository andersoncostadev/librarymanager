using FluentValidation;

namespace Core.Commands.v1.Book.Update
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(BeAValidGuid).WithMessage("Invalid GUID format.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("The Title is required.")
                .MaximumLength(100);

            RuleFor(x => x.Author)
                .NotEmpty()
                .WithMessage("The Author is required.")
                .MaximumLength(100);

            RuleFor(x => x.ISBN)
                .NotEmpty()
                .WithMessage("The ISBN is required.")
                .MaximumLength(100);

            RuleFor(x => x.YearPublication)
                .NotEmpty()
                .WithMessage("The Year Publication is require.")
                .InclusiveBetween(1000, 9999); ;
        }

        private bool BeAValidGuid(Guid id)
        {
            return Guid.TryParse(id.ToString(), out _);
        }
    }
}
