using MediatR;

namespace Core.Queries.v1.Book.GetId
{
    public class GetIdBookQuery : IRequest<GetIdBookQueryResponse>
    {
        public GetIdBookQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
