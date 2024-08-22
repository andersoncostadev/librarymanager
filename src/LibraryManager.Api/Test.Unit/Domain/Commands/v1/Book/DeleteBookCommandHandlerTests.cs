using Core.Commands.v1.Book.Delete;
using Core.Entities.v1;
using Core.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;


namespace Test.Unit.Domain.Commands.v1.Book
{
    [TestFixture]
    public class DeleteBookCommandHandlerTests
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<IValidator<DeleteBookCommand>> _validatorMock;
        private DeleteBookCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _validatorMock = new Mock<IValidator<DeleteBookCommand>>();
            _handler = new DeleteBookCommandHandler(_bookRepositoryMock.Object, _validatorMock.Object);
        }

        [Test]
        public void Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var command = new DeleteBookCommand(Guid.Empty);

            var validationResult = new FluentValidation.Results.ValidationResult(new[] { new ValidationFailure("Id", "Id is required") });
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);


            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<FluentValidation.ValidationException>().WithMessage("Validation failed");
        }

        [Test]
        public void Handle_WhenBookNotFound_ShouldThrowApplicationException()
        {
            // Arrange
            var command = new DeleteBookCommand(Guid.NewGuid());

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((BookEntity)null);

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("Book not found");
        }

        [Test]
        public async Task Handle_WhenBookIsDeletedSuccessfully_ShouldReturnSuccessResponse()
        {
            // Arrange
            var command = new DeleteBookCommand(Guid.NewGuid());

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var book = new BookEntity
            {
                Id = command.Id,
                Title = "Test Book",
                Author = "Author Name",
                ISBN = "1234567890",
                YearPublication = 2023
            };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(book);
            _bookRepositoryMock.Setup(r => r.DeleteAsync(command.Id)).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            _bookRepositoryMock.Verify(r => r.DeleteAsync(command.Id), Times.Once);
        }

        [Test]
        public void Handle_WhenDeleteFails_ShouldThrowApplicationException()
        {
            // Arrange
            var command = new DeleteBookCommand(Guid.NewGuid());

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var book = new BookEntity
            {
                Id = command.Id,
                Title = "Test Book",
                Author = "Author Name",
                ISBN = "1234567890",
                YearPublication = 2023
            };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(book);
            _bookRepositoryMock.Setup(r => r.DeleteAsync(command.Id)).ReturnsAsync(false);

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("Error deleting book");
        }
    }
}
