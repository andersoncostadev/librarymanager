namespace Core.Commands.v1.Loan.Create
{
    public class CreateLoanCommandResponse
    {
        public CreateLoanCommandResponse(Guid id, Guid bookId, Guid userId, DateTime loanDate, DateTime returnDate)
        {
            Id = id;
            BookId = bookId;
            UserId = userId;
            LoanDate = loanDate;
            ReturnDate = returnDate;
        }

        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
