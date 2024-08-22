using Core.Repositories;
using FluentValidation;
using MediatR;

namespace Core.Commands.v1.User.Delete
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<DeleteUserCommand> _validator;

        public DeleteUserCommandHandler(IUserRepository userRepository, IValidator<DeleteUserCommand> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new ApplicationException("User not found");

            await _userRepository.DeleteAsync(user.Id);

            return new DeleteUserCommandResponse();
        }
    }
}
