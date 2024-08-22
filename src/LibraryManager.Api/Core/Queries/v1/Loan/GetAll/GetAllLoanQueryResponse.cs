using Core.Entities.v1;

namespace Core.Queries.v1.Loan.GetAll
{
    public class GetAllLoanQueryResponse 
    {
        public GetAllLoanQueryResponse(IEnumerable<LoanEntity>? loans)
        {
            Loans = loans;
        }

        public IEnumerable<LoanEntity>? Loans { get; set; }
    }
}
