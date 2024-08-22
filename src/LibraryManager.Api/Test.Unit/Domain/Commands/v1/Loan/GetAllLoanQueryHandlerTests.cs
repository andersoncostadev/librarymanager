using Core.Entities.v1;
using Core.Queries.v1.Loan.GetAll;
using Core.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Unit.Domain.Commands.v1.Loan
{
    [TestFixture]
    public class GetAllLoanQueryHandlerTests
    {
        private Mock<ILoanRepository> _loanRepositoryMock;
        private GetAllLoanQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _handler = new GetAllLoanQueryHandler(_loanRepositoryMock.Object);
        }

        [Test]
        public async Task Handle_WhenLoansAreAvailable_ShouldReturnLoans()
        {
            // Arrange
            var loans = new List<LoanEntity>
            {
                new LoanEntity
                {
                    Id = Guid.NewGuid(),
                    BookId = Guid.NewGuid(),
                    LoanDate = DateTime.Now.AddDays(-10),
                    ReturnDate = DateTime.Now.AddDays(-5)
                },
                new LoanEntity
                {
                    Id = Guid.NewGuid(),
                    BookId = Guid.NewGuid(),
                    LoanDate = DateTime.Now.AddDays(-10),
                    ReturnDate = DateTime.Now.AddDays(-5)
                }
            };

            _loanRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(loans);

            var query = new GetAllLoanQuery();

            // Act
            var result = await _handler.Handle(new GetAllLoanQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Loans.Should().NotBeNull();
            result.Loans.Should().HaveCount(2);
            result.Loans.Should().Contain(loans);

            _loanRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public void Handle_WhenLoansAreNull_ShouldThrowApplicationException()
        {
            // Arrange
            _loanRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync((IEnumerable<LoanEntity>)null);

            var query = new GetAllLoanQuery();

            //Act & Assert
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("Error getting loans");

            _loanRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}
