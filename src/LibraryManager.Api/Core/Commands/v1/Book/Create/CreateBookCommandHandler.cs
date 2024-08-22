using Core.Entities.v1;
using Core.Enums.v1;
using Core.Repositories;
using FluentValidation;
using MediatR;

namespace Core.Commands.v1.Book.Create
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, CreateBookCommandResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IValidator<CreateBookCommand> _validator;
        public CreateBookCommandHandler(IBookRepository bookRepository, IValidator<CreateBookCommand> validator)
        {
            _bookRepository = bookRepository;
            _validator = validator;
        }

        public async Task<CreateBookCommandResponse> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var book = new BookEntity
            {
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                YearPublication = request.YearPublication,
                Status = BookStatus.Available
            };

            var createdBook = await _bookRepository.AddAsync(book);

            if (createdBook == null)
                throw new ApplicationException("Error creating book");

            return new CreateBookCommandResponse(book.Id, book.Title,book.Author, book.ISBN, book.YearPublication, book.Status);
        }
    }
}
