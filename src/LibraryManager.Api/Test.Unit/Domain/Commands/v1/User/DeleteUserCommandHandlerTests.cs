using Core.Commands.v1.User.Delete;
using Core.Entities.v1;
using Core.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Test.Unit.Domain.Commands.v1.User
{
    [TestFixture]
    public class DeleteUserCommandHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IValidator<DeleteUserCommand>> _validatorMock;
        private DeleteUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<DeleteUserCommand>>();
            _handler = new DeleteUserCommandHandler(_userRepositoryMock.Object, _validatorMock.Object);
        }

        [Test]
        public void Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            //Arrange
            var command = new DeleteUserCommand(Guid.Empty);

            var validationResult = new ValidationResult(new[]
            { new ValidationFailure("Id", "Id is required") });

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ValidationException>().WithMessage("Validation failed");
        }

        [Test]
        public void Handle_WhenUserNotFound_ShouldThrowApplicationException()
        {
            //Arrange
            var command = new DeleteUserCommand(Guid.NewGuid());

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            _userRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((Core.Entities.v1.UserEntity)null);

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("User not found");
        }

        [Test]
        public async Task Handle_WhenUserIsDeletedSuccessfully_ShouldReturnSuccess()
        {
            //Arrange
            var command = new DeleteUserCommand(Guid.NewGuid());

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var userEntity = new UserEntity
            {
                Id = command.Id,
                Name = "John Doe",
                Email = "john.doe@example.com"
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(userEntity);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            _userRepositoryMock.Verify(r => r.DeleteAsync(command.Id), Times.Once);
        }
    }
}
