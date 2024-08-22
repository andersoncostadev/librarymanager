namespace Core.Commands.v1.User.Create
{
    public class CreateUserCommandResponse
    {
        public CreateUserCommandResponse(Guid id, string? name, string? email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
