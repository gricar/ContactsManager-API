using Bogus;
using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Application.Usecases.Contacts.Commands.Delete;
using Contacts37.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Contacts37.Application.Tests.Usecases.Contacts.Commands.Delete
{
    public class DeleteContactCommandHandlerTests
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly DeleteContactCommandHandler _handler;
        private readonly Faker _faker;

        public DeleteContactCommandHandlerTests()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _handler = new DeleteContactCommandHandler(_contactRepositoryMock.Object);
            _faker = new Faker();
        }

        [Fact(DisplayName = "Validate contact deletion when contact exists")]
        [Trait("Category", "Delete Contact - Success")]
        public async void DeleteContact_ShouldSucceed_WhenContactExists()
        {
            // Arrange
            string name = _faker.Person.FirstName;
            int dddCode = _faker.PickRandom(new[] { 11, 21, 31, 41 });
            string phone = _faker.Phone.PhoneNumber("#########");
            string email = _faker.Internet.Email();

            var contact = Contact.Create(name, dddCode, phone, email);

            _contactRepositoryMock.Setup(repo => repo.GetAsync(contact.Id))
                .ReturnsAsync(contact);

            _contactRepositoryMock.Setup(repo => repo.DeleteAsync(contact))
                .Returns(Task.CompletedTask);

            var command = new DeleteContactCommand(contact.Id);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();
            _contactRepositoryMock.Verify(repo => repo.GetAsync(contact.Id), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.DeleteAsync(contact), Times.Once);
        }

        [Fact(DisplayName = "Validate contact deletion when contact does not exists")]
        [Trait("Category", "Delete Contact - Failure")]
        public async void DeleteContact_ShouldThrowException_WhenContactDoesNotExists()
        {
            // Arrange
            var contactId = _faker.Random.Guid();

            _contactRepositoryMock.Setup(repo => repo.GetAsync(contactId))
                .ReturnsAsync((Contact)null);

            var command = new DeleteContactCommand(contactId);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ContactNotFoundException>()
                .WithMessage($"Contact with ID '{contactId}' not found.");
            _contactRepositoryMock.Verify(repo => repo.GetAsync(contactId), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Contact>()), Times.Never);
        }
    }
}
