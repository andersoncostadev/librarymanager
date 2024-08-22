using MediatR;

namespace Core.Commands.v1.User.Create
{
    public class CreateUserCommand : IRequest<CreateUserCommandResponse>
    {
        public CreateUserCommand(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; set; }
        public string Email { get; set; }
    }
}
