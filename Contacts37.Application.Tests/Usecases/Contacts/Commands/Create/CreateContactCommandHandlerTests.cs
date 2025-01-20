using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Application.Tests.Fixtures;
using Contacts37.Application.Usecases.Contacts.Commands.Create;
using Contacts37.Domain.Entities;
using Contacts37.Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace Contacts37.Application.Tests.Usecases.Contacts.Commands.Create
{
    [Collection(nameof(ContactFixtureCollection))]
    public class CreateContactCommandHandlerTests
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly CreateContactCommandHandler _handler;
        private readonly ContactFixture _fixture;

        public CreateContactCommandHandlerTests(ContactFixture fixture)
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _handler = new CreateContactCommandHandler(_contactRepositoryMock.Object, fixture.Mapper);
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should create a contact successfully when data is valid and unique")]
        [Trait("Category", "Create Contact - Success")]
        public async Task CreateContact_ShouldSucess_WhenDataIsValidAndUnique()
        {
            // Arrange
            var contact = _fixture.CreateValidContact();
            var command = _fixture.CreateContactCommandFromEntity(contact);

            _contactRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(contact.Email!))
                .ReturnsAsync(true);

            _contactRepositoryMock.Setup(repo => repo.IsDddAndPhoneUniqueAsync(contact.Region.DddCode, contact.Phone))
                .ReturnsAsync(true);

            _contactRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Contact>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreateContactCommandResponse>();
            result.Id.Should().NotBeEmpty();

            _contactRepositoryMock.Verify(repo => repo.IsEmailUniqueAsync(command.Email!), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Once);
        }


        [Fact(DisplayName = "Should create a contact when email is null")]
        [Trait("Category", "Create Contact - Success")]
        public async void CreateContact_ShouldSucess_WhenEmailIsNull()
        {
            // Arrange
            var command = _fixture.CreateValidContactCommandWithEmailNull();

            _contactRepositoryMock.Setup(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone)).ReturnsAsync(true);
            _contactRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Contact>())).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreateContactCommandResponse>();
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Once);
        }

        [Fact(DisplayName = "Should fail to create contact when Phone is not unique")]
        [Trait("Category", "Create Contact - Failure - Phone already exists")]
        public async void CreateContact_ShouldThrowException_WhenPhoneIsNotUnique()
        {
            // Arrange
            var command = _fixture.CreateValidContactCommand();

            _contactRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(command.Email!))
                .ReturnsAsync(true);

            _contactRepositoryMock.Setup(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateContactException>()
                .WithMessage($"A contact with the same DDD '{command.DDDCode}' and phone '{command.Phone}' already exists.");

            _contactRepositoryMock.Verify(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Never);
        }


        [Fact(DisplayName = "Should fail to create contact when email is not unique")]
        [Trait("Category", "Create Contact - Failure - Email already exists")]
        public async Task CreateContact_ShouldThrowException_WhenEmailIsNotUnique()
        {
            // Arrange
            var command = _fixture.CreateValidContactCommand();

            _contactRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(command.Email!))
                .ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateEmailException>()
                .WithMessage($"A contact with the same Email '{command.Email}' already exists.");

            _contactRepositoryMock.Verify(repo => repo.IsEmailUniqueAsync(command.Email!), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Never);
        }

        [Fact(DisplayName = "Should fail to create contact when name is invalid")]
        [Trait("Category", "Create Contact - Failure - Invalid Name")]
        public async Task CreateContact_ShouldThrowException_WhenNameIsInvalid()
        {
            // Arrange
            var command = _fixture.CreateContactCommandWithInvalidData();

            _contactRepositoryMock.Setup(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidNameException>()
                .WithMessage("Name is required.");

            _contactRepositoryMock.Verify(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Never);
        }

        [Fact(DisplayName = "Should fail to create contact when email is invalid")]
        [Trait("Category", "Create Contact - Failure - Invalid Email")]
        public async Task CreateContact_ShouldThrowException_WhenEmailIsInvalid()
        {
            // Arrange
            var invalidEmail = "invalid_email";
            var contact = _fixture.CreateValidContact();
            var command = _fixture.CreateContactCommandWithInvalidData(contact.Name, contact.Region.DddCode, contact.Phone, invalidEmail);

            _contactRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(command.Email!))
                .ReturnsAsync(true);
            _contactRepositoryMock.Setup(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidEmailException>()
                .WithMessage($"Email '{command.Email}' must be a valid format.");

            _contactRepositoryMock.Verify(repo => repo.IsEmailUniqueAsync(command.Email!), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Never);
        }

        [Fact(DisplayName = "Should fail to create contact when phone is invalid")]
        [Trait("Category", "Create Contact - Failure - Invalid Phone")]
        public async Task CreateContact_ShouldThrowException_WhenPhoneIsInvalid()
        {
            // Arrange
            var contact = _fixture.CreateValidContact();
            var command = _fixture.CreateContactCommandWithInvalidData(contact.Name);

            _contactRepositoryMock.Setup(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone))
                .ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidPhoneNumberException>()
                .WithMessage($"Phone '{command.Phone}' must be a 9-digit number.");

            _contactRepositoryMock.Verify(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Never);
        }
    }
}
