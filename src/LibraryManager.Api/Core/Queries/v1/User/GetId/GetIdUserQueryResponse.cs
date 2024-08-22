namespace Core.Queries.v1.User.GetId
{
    public class GetIdUserQueryResponse
    {
        public GetIdUserQueryResponse(string? name, string? email)
        {
            Name = name;
            Email = email;
        }

        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
