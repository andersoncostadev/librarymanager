using Core.Commands.v1.User.Create;
using Core.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Test.Unit.Domain.Commands.v1.User
{
    [TestFixture]
    public class CreateUserCommandHandlerTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IValidator<CreateUserCommand>> _validatorMock;
        private CreateUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<CreateUserCommand>>();
            _handler = new CreateUserCommandHandler(_userRepository.Object, _validatorMock.Object);
        }

        [Test]
        public void Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            // Arrange
            var command = new CreateUserCommand(string.Empty, "invalid-email");

            var validationResult = new ValidationResult(new[]
            { new ValidationFailure("Name", "Name is required"), new ValidationFailure("Email", "Invalid email address") });

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ValidationException>().WithMessage("Validation failed");
        }

        [Test]
        public async Task Handle_WhenUserIsCreatedSuccessfully_ShouldReturnUserDetails()
        {
            //Arrange
            var command = new CreateUserCommand("John Doe", "john.doe@example.com");

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var capturedUser = new Core.Entities.v1.UserEntity
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Email = command.Email
            };

            _userRepository.Setup(r => r.AddAsync(It.IsAny<Core.Entities.v1.UserEntity>()))
                .Callback<Core.Entities.v1.UserEntity>(user => capturedUser = user)
                .ReturnsAsync((Core.Entities.v1.UserEntity user) => user);

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(capturedUser.Id);
            result.Name.Should().Be(command.Name);
            result.Email.Should().Be(command.Email);

            _userRepository.Verify(r => r.AddAsync(It.Is<Core.Entities.v1.UserEntity>(u => u.Name == command.Name)), Times.Once);
        }

        [Test]
        public void Handle_WhenCreatingUserFails_ShouldThrowApplicationException()
        {
            // Arrange
            var command = new CreateUserCommand("John Doe", "john.doe@example.com");

            var validationResult = new ValidationResult();

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            _userRepository.Setup(r => r.AddAsync(It.IsAny<Core.Entities.v1.UserEntity>()))
                .ReturnsAsync((Core.Entities.v1.UserEntity)null);

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("Error creating user");
        }
    }
}
