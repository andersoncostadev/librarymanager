using Core.Repositories;
using MediatR;

namespace Core.Queries.v1.Loan.GetAll
{
    public class GetAllLoanQueryHandler : IRequestHandler<GetAllLoanQuery, GetAllLoanQueryResponse>
    {
        private readonly ILoanRepository _loanRepository;

        public GetAllLoanQueryHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<GetAllLoanQueryResponse> Handle(GetAllLoanQuery request, CancellationToken cancellationToken)
        {
            var loans = await _loanRepository.GetAllAsync();

            if (loans == null)
                throw new ApplicationException("Error getting loans");

            return new GetAllLoanQueryResponse(loans);
        }
    }
}
