using Core.Repositories;
using FluentValidation;
using MediatR;

namespace Core.Queries.v1.User.GetId
{
    public class GetIdUserQueryHandler : IRequestHandler<GetIdUserQuery, GetIdUserQueryResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<GetIdUserQuery> _validator;

        public GetIdUserQueryHandler(IUserRepository userRepository, IValidator<GetIdUserQuery> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<GetIdUserQueryResponse> Handle(GetIdUserQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new ApplicationException("User not found");

            return new GetIdUserQueryResponse(user.Name, user.Email);
        }
    }
}
