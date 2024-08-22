using Core.Enums.v1;
using Core.Repositories;
using MediatR;

namespace Core.Commands.v1.Loan.ReturnLoan
{
    public class ReturnLoanCommandHandler : IRequestHandler<ReturnLoanCommand, ReturnLoanCommandResponse>
    {
        public readonly ILoanRepository _loanRepository;
        public readonly IBookRepository _bookRepository;

        public ReturnLoanCommandHandler(ILoanRepository loanRepository, IBookRepository bookRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
        }

        public async Task<ReturnLoanCommandResponse> Handle(ReturnLoanCommand request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetByIdAsync(request.LoanId);

            if (loan == null)
                throw new Exception("Loan not found");

            var expectedReturnDate = loan.ReturnDate;

            var daysLate = (DateTime.Now.Date > expectedReturnDate) ? (DateTime.Now.Date - expectedReturnDate).Days: 0;

            var message = daysLate > 0 ? $"Book returned {daysLate} days late" : "Book returned on time";

            var book = await _bookRepository.GetByIdAsync(loan.BookId);
            if(book == null)
                throw new Exception("Book not found");

            book.Status = BookStatus.Available;
            await _bookRepository.UpdateAsync(book);

            await _loanRepository.UpdateAsync(loan);

            await _loanRepository.DeleteAsync(loan.Id);

            return new ReturnLoanCommandResponse
            {
                Success = true,
                Message = message,
                DaysLate = daysLate > 0 ? daysLate : 0
            };
        }

    }
}
