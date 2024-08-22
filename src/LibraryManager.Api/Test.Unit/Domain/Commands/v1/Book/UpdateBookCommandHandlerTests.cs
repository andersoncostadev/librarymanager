using Core.Commands.v1.Book.Update;
using Core.Entities.v1;
using Core.Repositories;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace Test.Unit.Domain.Commands.v1.Book
{
    [TestFixture]
    public class UpdateBookCommandHandlerTests
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<IValidator<UpdateBookCommand>> _validatorMock;
        private UpdateBookCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _validatorMock = new Mock<IValidator<UpdateBookCommand>>();
            _handler = new UpdateBookCommandHandler(_bookRepositoryMock.Object, _validatorMock.Object);
        }

        [Test]
        public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var command = new UpdateBookCommand("title", "author", "isbn", 2021)
            {
                Id = Guid.Empty
            };

            var validationResult = new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Id", "Id is required") });

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);


            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<FluentValidation.ValidationException>().WithMessage("Validation failed");
        }

        [Test]
        public void Handle_WhenBookNotFound_ShouldThrowApplicationException()
        {
            // Arrange
            var command = new UpdateBookCommand("title", "author", "isbn", 2021)
            {
                Id = Guid.NewGuid()
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var book = new BookEntity
            {
                Id = Guid.NewGuid(),
                Title = "title",
                Author = "author",
                ISBN = "isbn",
                YearPublication = 2021
            };

            var updatedBook = new BookEntity
            {
                Id = command.Id,
                Title = command.Title,
                Author = command.Author,
                ISBN = command.ISBN,
                YearPublication = command.YearPublication
            };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(book);

            _bookRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<BookEntity>())).ReturnsAsync(updatedBook);

            // Act 
            var result = _handler.Handle(command, CancellationToken.None).Result;

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(updatedBook.Title);
            result.Author.Should().Be(updatedBook.Author);
            result.ISBN.Should().Be(updatedBook.ISBN);
            result.YearPublication.Should().Be(updatedBook.YearPublication);

            _bookRepositoryMock.Verify(r => r.UpdateAsync(It.Is<BookEntity>(b => b.Title == command.Title)), Times.Once);
        }

        [Test]
        public void Handle_WhenUpdateFails_ShouldThrowApplicationException()
        {
            var command = new UpdateBookCommand("title", "author", "isbn", 2021)
            {
                Id = Guid.NewGuid()
            };

            var validationResult = new FluentValidation.Results.ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var book = new BookEntity
            {
                Id = Guid.NewGuid(),
                Title = "title",
                Author = "author",
                ISBN = "isbn",
                YearPublication = 2021
            };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(book);

            _bookRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<BookEntity>())).ReturnsAsync((BookEntity)null);

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("Error updating book");
        }
    }
}
