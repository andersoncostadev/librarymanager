using Core.Entities.v1;
using Core.Enums.v1;
using Core.Repositories;
using FluentValidation;
using MediatR;

namespace Core.Commands.v1.Loan.Create
{
    public class CreateLoanCommandHandler : IRequestHandler<CreateLoanCommand, CreateLoanCommandResponse>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IValidator<CreateLoanCommand> _validator;

        public CreateLoanCommandHandler(ILoanRepository loanRepository, IValidator<CreateLoanCommand> validator, IBookRepository bookRepository)
        {
            _loanRepository = loanRepository;
            _validator = validator;
            _bookRepository = bookRepository;
        }

        public async Task<CreateLoanCommandResponse> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
                throw new ApplicationException("Book not found");

            if (book.Status == BookStatus.Borrowed)
                throw new ApplicationException("Book is already loaned");

            var loan = new LoanEntity
            {
                BookId = request.BookId,
                UserId = request.UserId,
                LoanDate = request.LoanDate.Date,
                ReturnDate = request.ReturnDate.Date
            };

            if(loan == null)
                throw new ApplicationException("Error creating loan");

            await _loanRepository.AddAsync(loan);

            book.Status = BookStatus.Borrowed;
            await _bookRepository.UpdateAsync(book);

            return new CreateLoanCommandResponse(loan.Id, loan.BookId, loan.UserId, loan.LoanDate, loan.ReturnDate);
        }
    }
}
