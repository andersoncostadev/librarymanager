using Core.Entities.v1;
using Core.Enums.v1;
using Core.Queries.v1.Book.GetId;
using Core.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Test.Unit.Domain.Commands.v1.Book
{
    [TestFixture]
    public class GetIdBookQueryHandlerTest
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<IValidator<GetIdBookQuery>> _validatorMock;
        private GetIdBookQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _validatorMock = new Mock<IValidator<GetIdBookQuery>>();
            _handler = new GetIdBookQueryHandler(_bookRepositoryMock.Object, _validatorMock.Object);
        }

        [Test]
        public void Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            //Arrange
            var query = new GetIdBookQuery(Guid.Empty);

            var validationResult = new FluentValidation.Results.ValidationResult(new[] { new ValidationFailure("Id", "Id is required") });

            _validatorMock.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            //Act & Assert
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<FluentValidation.ValidationException>().WithMessage("Validation failed");
        }

        [Test]
        public void Handle_WhenBookNotFound_ShouldThrowApplicationException()
        {
            //Arrange
            var query = new GetIdBookQuery(Guid.NewGuid());

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(query.Id)).ReturnsAsync((BookEntity)null);

            //Act & Assert
            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("Book not found");
        }

        [Test]
        public async Task Handle_WhenBookIsFound_ShouldReturnBookDetails()
        {          
            //Arrange
            var query = new GetIdBookQuery(Guid.NewGuid());

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var book = new BookEntity
            {
                Id = query.Id,
                Title = "Title 1",
                Author = "Author 1",
                ISBN = "ISBN 1",
                YearPublication = 2021,
                Status = BookStatus.Available
            };

            _bookRepositoryMock.Setup(r => r.GetByIdAsync(query.Id)).ReturnsAsync(book);

            //Act
            var result = await _handler.Handle(query, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(book.Title);
            result.Author.Should().Be(book.Author);
            result.ISBN.Should().Be(book.ISBN);
            result.YearPublication.Should().Be(book.YearPublication);
            result.Status.Should().Be(book.Status);

            _bookRepositoryMock.Verify(x => x.GetByIdAsync(query.Id), Times.Once);
        }
    }
}
