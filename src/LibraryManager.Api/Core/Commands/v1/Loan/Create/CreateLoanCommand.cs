using MediatR;

namespace Core.Commands.v1.Loan.Create
{
    public class CreateLoanCommand : IRequest<CreateLoanCommandResponse>
    {
        public CreateLoanCommand(Guid bookId, Guid userId, DateTime loanDate, DateTime returnDate)
        {
            BookId = bookId;
            UserId = userId;
            LoanDate = loanDate;
            ReturnDate = returnDate;
        }

        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
