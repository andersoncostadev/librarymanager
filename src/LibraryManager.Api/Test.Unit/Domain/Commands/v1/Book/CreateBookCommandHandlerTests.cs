using Core.Commands.v1.Book.Create;
using Core.Entities.v1;
using Core.Enums.v1;
using Core.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Test.Unit.Domain.Commands.v1.Book
{
    [TestFixture]
    public class CreateBookCommandHandlerTests
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<IValidator<CreateBookCommand>> _validatorMock;
        private CreateBookCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _validatorMock = new Mock<IValidator<CreateBookCommand>>();
            _handler = new CreateBookCommandHandler(_bookRepositoryMock.Object, _validatorMock.Object);
        }

        [Test]
        public async Task Handle_WhenCommandIsValid_ShouldCreateBook()
        {
            // Arrange
            var command = new CreateBookCommand
            {
                Title = "Test Book",
                Author = "Author Name",
                ISBN = "1234567890",
                YearPublication = 2023
            };

            var validationResult = new ValidationResult();
            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            BookEntity capturedBook = null;

            _bookRepositoryMock.Setup(x => x.AddAsync(It.IsAny<BookEntity>()))
                .Callback<BookEntity>(book => capturedBook = book)
                .ReturnsAsync((BookEntity book) => book);
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            capturedBook.Should().NotBeNull();
            result.Id.Should().Be(capturedBook.Id);
            result.Title.Should().Be(command.Title);
            result.Author.Should().Be(command.Author);
            result.ISBN.Should().Be(command.ISBN);
            result.YearPublication.Should().Be(command.YearPublication);
            result.Status.Should().Be(BookStatus.Available);

            _bookRepositoryMock.Verify(r => r.AddAsync(It.Is<BookEntity>(b => b.Title == command.Title)), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowApplicationException_WhenBookCreationFails()
        {
            // Arrange
            var command = new CreateBookCommand
            {
                Title = "Test Book",
                Author = "Author Name",
                ISBN = "1234567890",
                YearPublication = 2023
            };

            var validationResult = new ValidationResult();
            _validatorMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            _bookRepositoryMock.Setup(x => x.AddAsync(It.IsAny<BookEntity>()))
                .ReturnsAsync((BookEntity)null);

            // Act & Assert
            Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
