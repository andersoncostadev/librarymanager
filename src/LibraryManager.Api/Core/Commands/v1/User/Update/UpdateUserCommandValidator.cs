using FluentValidation;

namespace Core.Commands.v1.User.Update
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("The Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("The Email is required.")
                .MaximumLength(100)
                .EmailAddress();
        }
    }
}
