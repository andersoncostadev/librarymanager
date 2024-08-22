using Core.Repositories;
using FluentValidation;
using MediatR;

namespace Core.Commands.v1.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UpdateUserCommand> _validator;

        public UpdateUserCommandHandler(IUserRepository userRepository, IValidator<UpdateUserCommand> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new ApplicationException("User not found");

            user.Name = request.Name;
            user.Email = request.Email;

            var updatedUser = await _userRepository.UpdateAsync(user);

            if (updatedUser == null)
                throw new ApplicationException("Error updating user");

            return new UpdateUserCommandResponse(updatedUser.Name!, updatedUser.Email!);
        }
    }
}
