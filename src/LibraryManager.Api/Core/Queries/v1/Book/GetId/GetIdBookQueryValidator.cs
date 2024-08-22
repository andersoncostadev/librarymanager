using FluentValidation;

namespace Core.Queries.v1.Book.GetId
{
    public class GetIdBookQueryValidator : AbstractValidator<GetIdBookQuery>
    {
        public GetIdBookQueryValidator()
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