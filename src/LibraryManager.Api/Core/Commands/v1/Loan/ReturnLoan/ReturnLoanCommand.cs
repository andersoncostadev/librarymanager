using MediatR;

namespace Core.Commands.v1.Loan.ReturnLoan
{
    public class ReturnLoanCommand : IRequest<ReturnLoanCommandResponse>
    {
        public ReturnLoanCommand(Guid loanId)
        {
            LoanId = loanId;
        }

        public Guid LoanId { get; set; }
    }
}
