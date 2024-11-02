using Bogus;
using Contacts37.Application.Contracts.Persistence;
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

        [Fact(DisplayName = "Validate contact deletion")]
        [Trait("Category", "Delete Contact - Success")]
        public async void HandleDeleteContact_Successful()
        {
            // Arrange
            var contactId = _faker.Random.Guid();
            var contact = new Contact { Id = contactId };

            _contactRepositoryMock.Setup(repo => repo.GetAsync(contactId))
                .Returns(Task.FromResult(contact));

            _contactRepositoryMock.Setup(repo => repo.DeleteAsync(contact))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await action.Should().NotThrowAsync();
            _contactRepositoryMock.Verify(repo => repo.GetAsync(contactId), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.DeleteAsync(contact), Times.Once);
        }
    }
}
