using Core.Commands.v1.Loan.Create;
using Core.Entities.v1;
using Core.Enums.v1;
using Core.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Test.Unit.Domain.Commands.v1.Loan
{
    [TestFixture]
    public class CreateLoanCommandHandlerTests
    {
        private Mock<ILoanRepository> _loanRepositoryMock;
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<IValidator<CreateLoanCommand>> _validatorMock;
        private CreateLoanCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _validatorMock = new Mock<IValidator<CreateLoanCommand>>();
            _handler = new CreateLoanCommandHandler(_loanRepositoryMock.Object, _validatorMock.Object, _bookRepositoryMock.Object);
        }

        [Test]
        public void Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            //Arrange
            var command = new CreateLoanCommand(Guid.Empty, Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(7));

            var validationResult = new FluentValidation.Results.ValidationResult(new[] { new ValidationFailure("BookId", "BookId is required") });

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            //Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<FluentValidation.ValidationException>().WithMessage("Validation failed");
        }

        [Test]
        public void Handle_WhenBookNotFound_ShouldThrowApplicationException()
        {
            //Arrange
            var command = new CreateLoanCommand(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(7));

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(command.BookId)).ReturnsAsync((BookEntity)null);

            //Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("Book not found");
        }

        [Test]
        public void Handle_WhenBookIsAlreadyLoaned_ShouldThrowApplicationException()
        {             
            //Arrange
            var command = new CreateLoanCommand(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(7));

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var bookEntity = new BookEntity
            {
                Id = command.BookId,
                Status = BookStatus.Borrowed
            };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(command.BookId)).ReturnsAsync(bookEntity);

            //Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("Book is already loaned");
        }

        [Test]
        public async Task Handle_WhenLoanIsCreated_ShouldReturnLoanDetailsAsync()
        {
            //Arrange
            var command = new CreateLoanCommand(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(7));

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var bookEntity = new BookEntity
            {
                Id = command.BookId,
                Status = BookStatus.Available
            };

            var loanEntity = new LoanEntity
            {
                Id = Guid.NewGuid(),
                BookId = command.BookId,
                UserId = command.UserId,
                LoanDate = command.LoanDate.Date,
                ReturnDate = command.ReturnDate.Date,
            };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(command.BookId)).ReturnsAsync(bookEntity);

            LoanEntity capturedLoanEntity = null;
            _loanRepositoryMock.Setup(r => r.AddAsync(It.IsAny<LoanEntity>()))
                       .Callback<LoanEntity>(loan => capturedLoanEntity = loan)
                       .ReturnsAsync(loanEntity);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(capturedLoanEntity.Id);
            result.BookId.Should().Be(loanEntity.BookId);
            result.UserId.Should().Be(loanEntity.UserId);
            result.LoanDate.Should().Be(loanEntity.LoanDate);
            result.ReturnDate.Should().Be(loanEntity.ReturnDate);

            _bookRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<BookEntity>()), Times.Once);
        }

        [Test]
        public void Handle_WhenCreatingLoanFails_ShouldThrowApplicationException()
        {           
            //Arrange
            var command = new CreateLoanCommand(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, DateTime.Now.AddDays(7));

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var bookEntity = new BookEntity
            {
                Id = command.BookId,
                Status = BookStatus.Available
            };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(command.BookId)).ReturnsAsync(bookEntity);

            _loanRepositoryMock.Setup(r => r.AddAsync(It.IsAny<LoanEntity>())).ReturnsAsync((LoanEntity)null);

            //Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("Error creating loan");
        }
    }
}
