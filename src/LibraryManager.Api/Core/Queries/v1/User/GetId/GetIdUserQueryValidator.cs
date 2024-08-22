using FluentValidation;

namespace Core.Queries.v1.User.GetId
{
    public class GetIdUserQueryValidator : AbstractValidator<GetIdUserQuery>
    {
        public GetIdUserQueryValidator()
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
