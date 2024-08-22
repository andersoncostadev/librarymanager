using Core.Entities.v1;
using Core.Queries.v1.User.GetAll;
using Core.Repositories;
using Moq;

namespace Test.Unit.Domain.Commands.v1.User
{
    [TestFixture]
    public class GetAllUserQueryHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private GetAllUserQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetAllUserQueryHandler(_userRepositoryMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnAllUsers()
        {
            //Arrange
            var users = new List<UserEntity>
            {
                new UserEntity { Id = Guid.NewGuid(), Name = "User1", Email = "user1@example.com" },
                new UserEntity { Id = Guid.NewGuid(), Name = "User2", Email = "user2@example.com" }
            };

            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var query = new GetAllUserQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Users.Count());
            Assert.IsTrue(result.Users.Any(u => u.Name == "User1"));
            Assert.IsTrue(result.Users.Any(u => u.Name == "User2"));
        }

        [Test]
        public async Task Handle_WhenNoUsersFound_ShouldReturnEmptyList()
        {
            //Arrange
            _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync((IEnumerable<UserEntity>)null);

            var query = new GetAllUserQuery();

            //Assert & Act
            Assert.ThrowsAsync<ApplicationException>(async () => await _handler.Handle(query, CancellationToken.None));
        }
    }
}
