using FluentValidation;

namespace Core.Commands.v1.Book.Create
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("The Title is required.")
                .MaximumLength(100);

            RuleFor(x => x.Author)
                .NotEmpty()
                .WithMessage("The Author is required.")
                .MaximumLength(100); ;

            RuleFor(x => x.ISBN)
                .NotEmpty()
                .WithMessage("The ISBN is required.")
                .MaximumLength(50);

            RuleFor(x => x.YearPublication)
                .NotEmpty()
                .WithMessage("The Year Publication is require.")
                .InclusiveBetween(1000, 9999);
        }
    }
}
