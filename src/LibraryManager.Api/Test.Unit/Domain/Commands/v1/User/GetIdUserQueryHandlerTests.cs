using Core.Entities.v1;
using Core.Queries.v1.User.GetId;
using Core.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Test.Unit.Domain.Commands.v1.User
{
    [TestFixture]
    public class GetIdUserQueryHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IValidator<GetIdUserQuery>> _validatorMock;
        private GetIdUserQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<GetIdUserQuery>>();
            _handler = new GetIdUserQueryHandler(_userRepositoryMock.Object, _validatorMock.Object);
        }

        [Test]
        public async Task Handle_UserExists_ShouldReturnUserResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new UserEntity { Id = userId, Name = "Test User", Email = "test@example.com" };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetIdUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var query = new GetIdUserQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(user.Name, result.Name);
            Assert.AreEqual(user.Email, result.Email);
        }

        [Test]
        public void Handle_UserDoesNotExist_ShouldThrowException()
        {   
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((UserEntity)null);

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetIdUserQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var query = new GetIdUserQuery(userId);

            // Act & Assert
            Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
