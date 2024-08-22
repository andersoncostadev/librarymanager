using MediatR;

namespace Core.Commands.v1.User.Update
{
    public class UpdateUserCommand : IRequest<UpdateUserCommandResponse>
    {
        public UpdateUserCommand(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
