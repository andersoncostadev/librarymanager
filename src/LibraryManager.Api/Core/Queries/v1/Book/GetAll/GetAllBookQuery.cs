using MediatR;

namespace Core.Queries.v1.Book.GetAll
{
    public class GetAllBookQuery : IRequest<GetAllBookQueryResponse>
    {
    }
}
