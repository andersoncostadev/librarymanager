using MediatR;

namespace Core.Queries.v1.User.GetId
{
    public class GetIdUserQuery : IRequest<GetIdUserQueryResponse>
    {
        public GetIdUserQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
