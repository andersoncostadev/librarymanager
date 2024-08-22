using Core.Repositories;
using MediatR;

namespace Core.Queries.v1.Book.GetAll
{
    public class GetAllBookQueryHandler : IRequestHandler<GetAllBookQuery, GetAllBookQueryResponse>
    {
        private readonly IBookRepository _bookRepository;

        public GetAllBookQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<GetAllBookQueryResponse> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAllAsync();

            if (books == null)
                throw new ApplicationException("Error getting books");

            return new GetAllBookQueryResponse(books);
        }
    }
}
