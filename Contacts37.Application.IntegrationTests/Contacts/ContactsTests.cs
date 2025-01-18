using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.IntegrationTests.Fixtures;
using Contacts37.Application.IntegrationTests.Infrastructure;
using Contacts37.Application.Usecases.Contacts.Commands.Update;
using Contacts37.Application.Usecases.Contacts.Queries.GetAll;
using Contacts37.Application.Usecases.Contacts.Queries.GetByDdd;
using Contacts37.Domain.Entities;
using FluentAssertions;

namespace Contacts37.Application.IntegrationTests.Contacts
{
    [Collection(nameof(IntegrationTestCollection))]
    public class ContactsTests : BaseIntegrationTest
    {
        private readonly ContactFixture _fixture;
        public ContactsTests(IntegrationTestWebAppFactory factory, ContactFixture fixture) : base(factory)
        {
            _fixture = fixture;
        }


        [Fact(DisplayName = "Should create new contact with valid values")]
        [Trait("Category", "Integration")]
        [Trait("Component", "Database")]
        public async Task CreateContactCommand_ShouldAddNewContact_WhenValid()
        {
            //Arrange
            var command = _fixture.CreateValidContactCommand();

            //Act
            var result = await Sender.Send(command);

            //Assert
            var contact = DbContext.Contacts.FirstOrDefault(c => c.Id == result.Id);

            contact.Should().NotBeNull()
                .And.BeOfType<Contact>();
            contact!.Name.Should().NotBeNullOrWhiteSpace()
                .And.Be(command.Name);
            contact.Region.DddCode.Should().BeGreaterThan(0)
                .And.Be(command.DDDCode);
            contact.Phone.Should().HaveLength(9)
                .And.Be(command.Phone);
            contact.Email.Should().NotBeNullOrWhiteSpace()
                .And.Be(command.Email);
        }

        [Fact(DisplayName = "Should throw exception when creating new contact with invalid values")]
        [Trait("Category", "Integration")]
        [Trait("Component", "Database")]
        public async Task CreateContactCommand_ShouldThrowException_WhenPhoneIsInvalid()
        {
            //Arrange
            var command = _fixture.CreateInvalidContactCommand();

            //Act
            Func<Task> act = async () => await Sender.Send(command);

            //Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact(DisplayName = "Should update existing contact with valid values")]
        [Trait("Category", "Integration")]
        [Trait("Component", "Database")]
        public async Task UpdateContactCommand_ShouldUpdateContact_WhenIsValid()
        {
            //Arrange
            var existingContact = DataSeeder.GetTestContact();
            var command = new UpdateContactCommand(existingContact.Id, "Ayrton", 11, "987654321", "ayrton.senna@example.com");

            //Act
            await Sender.Send(command);

            //Assert
            var contact = DbContext.Contacts.FirstOrDefault(c => c.Id == existingContact.Id);

            contact.Should().NotBeNull()
                .And.BeOfType<Contact>();
            contact!.Name.Should().NotBeNullOrWhiteSpace()
                    .And.Be("Ayrton");
            contact.Region.DddCode.Should().BeGreaterThan(0)
                .And.Be(11);
            contact.Phone.Should().HaveLength(9)
                .And.Be("987654321");
            contact.Email.Should().NotBeNullOrWhiteSpace()
                .And.Be("ayrton.senna@example.com");
        }

        [Fact(DisplayName = "Should return existing contacts from database")]
        [Trait("Category", "Integration")]
        [Trait("Component", "Database")]
        public async Task GetAllContactsCommand_ShouldReturnContactsList_FromDatabase()
        {
            //Act
            var result = await Sender.Send(new GetAllContactsRequest());

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5)
                .And.AllBeOfType<GetAllContactsResponse>();
        }

        [Fact(DisplayName = "Should return one contact that exists on database")]
        [Trait("Category", "Integration")]
        [Trait("Component", "Database")]
        public async Task GetContactsByDdd_ShouldReturnContacts_WhenContactsExists()
        {
            //Arrange
            var existingContact = DataSeeder.GetTestContact();
            var query = new GetContactsByDddRequest(existingContact.Region.DddCode);

            //Act
            var result = await Sender.Send(query);

            //Assert
            result.Should().NotBeNull()
                .And.AllBeOfType<GetContactsByDddResponse>()
                .And.ContainSingle(contact => contact.Name == existingContact.Name
                    && contact.DDDCode == existingContact.Region.DddCode);
        }

        [Fact(DisplayName = "Should return empty list of contact from the database")]
        [Trait("Category", "Integration")]
        [Trait("Component", "Database")]
        public async Task GetByDdd_ShouldReturnEmpty_WhenNoContactsForDddCode()
        {
            // Arrange
            var request = new GetContactsByDddRequest(99);

            // Act
            var result = await Sender.Send(request);

            // Assert
            result.Should().NotBeNull()
                  .And.BeEmpty();
        }
    }
}
