using Core.Repositories;
using FluentValidation;
using MediatR;

namespace Core.Commands.v1.Book.Delete
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, DeleteBookCommandResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IValidator<DeleteBookCommand> _validator;

        public DeleteBookCommandHandler(IBookRepository bookRepository, IValidator<DeleteBookCommand> validator)
        {
            _bookRepository = bookRepository;
            _validator = validator;
        }

        public async Task<DeleteBookCommandResponse> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
           var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var book = await _bookRepository.GetByIdAsync(request.Id);

            if (book == null)
                throw new ApplicationException("Book not found");

            var deletedBook = await _bookRepository.DeleteAsync(book.Id);

            if (!deletedBook)
                throw new ApplicationException("Error deleting book");

            return new DeleteBookCommandResponse();
        }
    }
}
