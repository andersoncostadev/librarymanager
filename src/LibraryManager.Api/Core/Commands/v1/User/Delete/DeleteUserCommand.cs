using MediatR;

namespace Core.Commands.v1.User.Delete
{
    public class DeleteUserCommand : IRequest<DeleteUserCommandResponse>
    {
        public DeleteUserCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
