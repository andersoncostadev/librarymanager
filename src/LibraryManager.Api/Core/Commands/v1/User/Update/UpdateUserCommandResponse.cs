namespace Core.Commands.v1.User.Update
{
    public class UpdateUserCommandResponse
    {
        public UpdateUserCommandResponse(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; set; }
        public string Email { get; set; }
    }
}
