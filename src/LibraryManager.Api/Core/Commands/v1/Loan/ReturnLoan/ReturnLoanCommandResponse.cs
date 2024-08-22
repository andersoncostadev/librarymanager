namespace Core.Commands.v1.Loan.ReturnLoan
{
    public class ReturnLoanCommandResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int DaysLate { get; set; }
    }
}
