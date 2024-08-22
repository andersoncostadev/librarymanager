using Core.Repositories;
using FluentValidation;
using MediatR;

namespace Core.Commands.v1.Book.Update
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, UpdateBookCommandResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IValidator<UpdateBookCommand> _validator;

        public UpdateBookCommandHandler(IBookRepository bookRepository, IValidator<UpdateBookCommand> validator)
        {
            _bookRepository = bookRepository;
            _validator = validator;
        }

        public async Task<UpdateBookCommandResponse> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var book = await _bookRepository.GetByIdAsync(request.Id);

            if (book == null)
                throw new ApplicationException("Book not found");

            book.Title = request.Title;
            book.Author = request.Author;
            book.ISBN = request.ISBN;
            book.YearPublication = request.YearPublication;

            var updatedBook = await _bookRepository.UpdateAsync(book);

            if (updatedBook == null)
                throw new ApplicationException("Error updating book");

            return new UpdateBookCommandResponse( updatedBook.Title, updatedBook.Author, updatedBook.ISBN, updatedBook.YearPublication);
        }
    }
}
