using Core.Commands.v1.User.Update;
using Core.Entities.v1;
using Core.Repositories;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Constraints.Tolerance;

namespace Test.Unit.Domain.Commands.v1.User
{
    [TestFixture]
    public class UpdateUserCommandHandlerTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IValidator<UpdateUserCommand>> _validatorMock;
        private UpdateUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _validatorMock = new Mock<IValidator<UpdateUserCommand>>();
            _handler = new UpdateUserCommandHandler(_userRepositoryMock.Object, _validatorMock.Object);
        }

        [Test]
        public void Handle_WhenValidationFails_ShouldThrowValidationException()
        {
            //Arrange
            var command = new UpdateUserCommand(string.Empty, "invalid-email") { Id = Guid.NewGuid() };

            var validationResult = new ValidationResult(new[]
            { new ValidationFailure("Name", "Name is required"),
              new ValidationFailure("Email", "Invalid email")
            });

            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ValidationException>().WithMessage("Validation failed");
        }

        [Test]
        public void Handle_WhenUserNotFound_ShouldThrowApplicationException()
        {
            // Arrange
            var command = new UpdateUserCommand("John Doe", "john.doe@example.com") { Id = Guid.NewGuid() };

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            _userRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((Core.Entities.v1.UserEntity)null);

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("User not found");
        }

        [Test]
        public async Task Handle_WhenUserIsUpdatedSuccessfully_ShouldReturnUpdatedUserDetails()
        {
            // Arrange
            var command = new UpdateUserCommand("John Doe", "john.doe@example.com") { Id = Guid.NewGuid() };

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var userEntity = new UserEntity
            {
                Id = command.Id,
                Name = "John Doe",
                Email = "old.email@example.com"
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(userEntity);

            _userRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<UserEntity>()))
                               .ReturnsAsync(userEntity);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(command.Name);
            result.Email.Should().Be(command.Email);

            _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<UserEntity>(u => u.Name == command.Name && u.Email == command.Email)), Times.Once);
        }

        [Test]
        public void Handle_WhenUpdatingUserFails_ShouldThrowApplicationException()
        {
            // Arrange
            var command = new UpdateUserCommand("John Doe", "john.doe@example.com") { Id = Guid.NewGuid() };

            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResult);

            var userEntity = new UserEntity
            {
                Id = command.Id,
                Name = "Old Name",
                Email = "old.email@example.com"
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(userEntity);

            _userRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<UserEntity>()))
                               .ReturnsAsync((UserEntity)null);

            // Act & Assert
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
            act.Should().ThrowAsync<ApplicationException>().WithMessage("Error updating user");
        }
    }
}

