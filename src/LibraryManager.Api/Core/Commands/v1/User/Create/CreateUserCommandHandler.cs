using Core.Entities.v1;
using Core.Repositories;
using FluentValidation;
using MediatR;

namespace Core.Commands.v1.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse>
    {
        public readonly IUserRepository _userRepository;
        public readonly IValidator<CreateUserCommand> _validator;

        public CreateUserCommandHandler(IUserRepository userRepository, IValidator<CreateUserCommand> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = new UserEntity
            {
                Name = request.Name,
                Email = request.Email
            };

            var createdUser = await _userRepository.AddAsync(user);

            if (createdUser == null)
                throw new ApplicationException("Error creating user");

            return new CreateUserCommandResponse(user.Id, user.Name, user.Email);
        }
    }
}
