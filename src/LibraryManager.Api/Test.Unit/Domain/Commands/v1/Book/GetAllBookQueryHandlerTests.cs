using Core.Entities.v1;
using Core.Queries.v1.Book.GetAll;
using Core.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Unit.Domain.Commands.v1.Book
{
    [TestFixture]
    public class GetAllBookQueryHandlerTests
    {
        private Mock<IBookRepository> _bookRepository;
        private GetAllBookQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _bookRepository = new Mock<IBookRepository>();
            _handler = new GetAllBookQueryHandler(_bookRepository.Object);
        }

        [Test]
        public async Task Handle_WhenBooksAreAvailable_ShouldReturnBooks()
        {
            // Arrange
            var books = new List<BookEntity>
            {
                new BookEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Title 1",
                    Author = "Author 1",
                    ISBN = "ISBN 1",
                    YearPublication = 2021
                },
                new BookEntity
                {
                    Id = Guid.NewGuid(),
                    Title = "Title 2",
                    Author = "Author 2",
                    ISBN = "ISBN 2",
                    YearPublication = 2022
                }
            };

            _bookRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(books);

            var query = new GetAllBookQuery();

            // Act
            var result = await _handler.Handle(new GetAllBookQuery(), CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Books.Should().NotBeNull();
            result.Books.Should().HaveCount(2);
            result.Books.Should().Contain(books);

            _bookRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public void Handle_WhenBooksAreNull_ShouldThrowApplicationException()
        {
            // Arrange
            _bookRepository.Setup(x => x.GetAllAsync()).ReturnsAsync((IEnumerable<BookEntity>)null);

            var query = new GetAllBookQuery();

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(new GetAllBookQuery(), CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("Error getting books");

            _bookRepository.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }
}
