using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Application.Tests.Fixtures;
using Contacts37.Application.Usecases.Contacts.Commands.Update;
using Contacts37.Domain.Entities;
using Contacts37.Domain.Exceptions;
using FluentAssertions;
using MediatR;
using Moq;


namespace Contacts37.Application.Tests.Usecases.Contacts.Commands.Put
{
    [Collection(nameof(ContactFixtureCollection))]
    public class UpdateContactCommandHandlerTests
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly UpdateContactCommandHandler _handler;
        private readonly ContactFixture _fixture;

        public UpdateContactCommandHandlerTests(ContactFixture fixture)
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _handler = new UpdateContactCommandHandler(_contactRepositoryMock.Object);
            _fixture = fixture;
        }

        [Fact(DisplayName = "Validate contact update when contact exists")]
        [Trait("Category", "Update Contact - Success")]
        public async Task UpdateContact_ShouldSucess_WhenContactExists()
        {
            // Arrange
            var existingContact = _fixture.CreateValidContact();
            var command = _fixture.CreateValidUpdateContactCommand(existingContact);

            _contactRepositoryMock.Setup(repo => repo.GetAsync(existingContact.Id))
                .ReturnsAsync(existingContact);

            _contactRepositoryMock.Setup(repo => repo.IsDddAndPhoneUniqueAsync(existingContact.Region.DddCode, existingContact.Phone))
                .ReturnsAsync(true);

            _contactRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(existingContact.Email!))
                .ReturnsAsync(true);

            _contactRepositoryMock.Setup(repo => repo.UpdateAsync(existingContact))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _contactRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Contact>()), Times.Once);
        }

        [Fact(DisplayName = "Should throw Exception when contact is not found")]
        [Trait("Category", "Update Contact - Failure")]
        public async Task UpdateContact_ShouldThrowException_WhenContactDoesNotExist()
        {
            // Arrange
            var command = _fixture.CreateValidUpdateContactCommandForNonExistentContact();

            _contactRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Contact)null);

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ContactNotFoundException>()
                .WithMessage($"Contact with ID '{command.Id}' not found.");
        }


        [Fact(DisplayName = "Validate contact update when invalid email")]
        [Trait("Category", "Update Contact - Failure - Invalid Email")]
        public async void UpdateContact_ShouldThrowException_WhenContactInvalidEmail()
        {
            // Arrange
            var contact = _fixture.CreateValidContact();
            var command = _fixture.CreateInvalidUpdateContactCommandWithInvalidEmail(contact);

            _contactRepositoryMock.Setup(repo => repo.GetAsync(contact.Id))
                .ReturnsAsync(contact);

            _contactRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(command.Email!))
                .ReturnsAsync(true);

            _contactRepositoryMock.Setup(repo => repo.UpdateAsync(contact));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidEmailException>()
                .WithMessage($"Email '{command.Email!}' must be a valid format.");
            _contactRepositoryMock.Verify(repo => repo.GetAsync(contact.Id), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.UpdateAsync(contact), Times.Never);
        }

        [Fact(DisplayName = "Validate contact update when phone is not unique")]
        [Trait("Category", "Update Contact - Failure - Duplicated Phone")]
        public async void UpdateContact_ShouldThrowException_WhenContactDuplicatedPhone()
        {
            // Arrange
            var contact = _fixture.CreateValidContact();
            var command = _fixture.CreateValidUpdateContactCommandWithNewPhone(contact);

            _contactRepositoryMock.Setup(repo => repo.GetAsync(contact.Id))
                .ReturnsAsync(contact);

            _contactRepositoryMock.Setup(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone))
                .ReturnsAsync(false);

            _contactRepositoryMock.Setup(repo => repo.UpdateAsync(contact));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateContactException>()
                .WithMessage($"A contact with the same DDD '{command.DDDCode}' and phone '{command.Phone}' already exists.");
            _contactRepositoryMock.Verify(repo => repo.GetAsync(contact.Id), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode, command.Phone), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.UpdateAsync(contact), Times.Never);
        }
    }
}
