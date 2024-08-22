using Core.Commands.v1.Loan.Create;
using Core.Commands.v1.Loan.ReturnLoan;
using Core.Queries.v1.Loan.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoanController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] CreateLoanCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLoans()
        {
            var query = new GetAllLoanQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPut("return/id")]
        public async Task<IActionResult> ReturnLoan(Guid id)
        {
            var command = new ReturnLoanCommand(id);

            var response = await _mediator.Send(command);

            if(!response.Success)
                return BadRequest(response.Message);

            return Ok(new
            {
                message = response.Message,
                daysLate = response.DaysLate
            });
        }
    }
}
