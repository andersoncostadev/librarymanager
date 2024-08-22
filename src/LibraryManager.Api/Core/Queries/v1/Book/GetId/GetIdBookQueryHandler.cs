using Core.Repositories;
using FluentValidation;
using MediatR;

namespace Core.Queries.v1.Book.GetId
{
    public class GetIdBookQueryHandler : IRequestHandler<GetIdBookQuery, GetIdBookQueryResponse>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IValidator<GetIdBookQuery> _validator;

        public GetIdBookQueryHandler(IBookRepository bookRepository, IValidator<GetIdBookQuery> validator)
        {
            _bookRepository = bookRepository;
            _validator = validator;
        }

        public async Task<GetIdBookQueryResponse> Handle(GetIdBookQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var book = await _bookRepository.GetByIdAsync(request.Id);

            if (book == null)
                throw new ApplicationException("Book not found");

            return new GetIdBookQueryResponse(book.Title, book.Author, book.ISBN, book.YearPublication, book.Status);
        }
    }
}
