using Core.Commands.v1.Loan.ReturnLoan;
using Core.Entities.v1;
using Core.Enums.v1;
using Core.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Unit.Domain.Commands.v1.Loan
{
    [TestFixture]
    public class ReturnLoanCommandHandlerTests
    {
        private Mock<ILoanRepository> _loanRepositoryMock;
        private Mock<IBookRepository> _bookRepositoryMock;
        private ReturnLoanCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _handler = new ReturnLoanCommandHandler(_loanRepositoryMock.Object, _bookRepositoryMock.Object);
        }

        [Test]
        public void Handle_WhenLoanNotFound_ShouldThrowException()
        {
            //Arrange
            var command = new ReturnLoanCommand(Guid.NewGuid());

            _loanRepositoryMock.Setup(r => r.GetByIdAsync(command.LoanId)).ReturnsAsync((LoanEntity)null);

            //Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<Exception>().WithMessage("Loan not found");
        }

        [Test]
        public void Handle_WhenBookNotFound_ShouldThrowException()
        {
            //Arrange
            var command = new ReturnLoanCommand(Guid.NewGuid());

            var loan = new LoanEntity
            {
                Id = command.LoanId,
                BookId = Guid.NewGuid(),
                LoanDate = DateTime.Now.AddDays(-10),
                ReturnDate = DateTime.Now.AddDays(-5)
            };

            _loanRepositoryMock.Setup(r => r.GetByIdAsync(command.LoanId)).ReturnsAsync(loan);

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((BookEntity)null);

            //Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<Exception>().WithMessage("Book not found");
        }

        [Test]
        public async Task Handle_WhenLoanIsReturnedOnTime_ShouldReturnSuccess()
        {
            //Arrange
            var command = new ReturnLoanCommand(Guid.NewGuid());

            var loan = new LoanEntity
            {
                Id = command.LoanId,
                BookId = Guid.NewGuid(),
                LoanDate = DateTime.Now.AddDays(-10),
                ReturnDate = DateTime.Now
            };

            var book = new BookEntity
            {
                Id = loan.BookId,
                Status = BookStatus.Borrowed
            };

            _loanRepositoryMock.Setup(r => r.GetByIdAsync(command.LoanId)).ReturnsAsync(loan);

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(loan.BookId)).ReturnsAsync(book);


            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Book returned on time");
            result.DaysLate.Should().Be(0);

            _bookRepositoryMock.Verify(r => r.UpdateAsync(It.Is<BookEntity>(b => b.Status == BookStatus.Available)), Times.Once);
            _loanRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<LoanEntity>()), Times.Once);
        }

        [Test]
        public async Task Handle_WhenLoanIsReturnedLate_ShouldReturnSuccessWithDaysLate()
        {
            //Arrange
            var command = new ReturnLoanCommand(Guid.NewGuid());

            var loan = new LoanEntity
            {
                Id = command.LoanId,
                BookId = Guid.NewGuid(),
                LoanDate = DateTime.Now.AddDays(-10).Date,
                ReturnDate = DateTime.Now.AddDays(-5).Date
            };

            var book = new BookEntity
            {
                Id = loan.BookId,
                Status = BookStatus.Borrowed
            };

            _loanRepositoryMock.Setup(r => r.GetByIdAsync(command.LoanId)).ReturnsAsync(loan);

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(loan.BookId)).ReturnsAsync(book);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.DaysLate.Should().Be(5);
            result.Message.Should().Be("Book returned 5 days late");


            _bookRepositoryMock.Verify(r => r.UpdateAsync(It.Is<BookEntity>(b => b.Status == BookStatus.Available)), Times.Once);
            _loanRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<LoanEntity>()), Times.Once);
        }
    }
}
