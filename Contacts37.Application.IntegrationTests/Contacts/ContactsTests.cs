using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.IntegrationTests.Infrastructure;
using Contacts37.Application.Usecases.Contacts.Commands.Create;
using Contacts37.Application.Usecases.Contacts.Queries.GetAll;
using Contacts37.Application.Usecases.Contacts.Queries.GetByDdd;
using Contacts37.Domain.Entities;
using FluentAssertions;

namespace Contacts37.Application.IntegrationTests.Contacts
{
    public class ContactsTests : BaseIntegrationTest
    {
        public ContactsTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }


        [Fact(DisplayName = "Should create new contact with valid values")]
        [Trait("Category", "Integration")]
        public async Task Create_ShouldAddContact_WhenCommandIsValid()
        {
            //Arrange
            var command = new CreateContactCommand("Ayrton", 11, "987654321", "ayrton.senna@example.com");

            //Act
            var result = await Sender.Send(command);

            //Assert
            var contact = DbContext.Contacts.FirstOrDefault(c => c.Id == result.Id);

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

        [Fact(DisplayName = "Should throw exception when creating new contact with invalid values")]
        [Trait("Category", "Integration")]
        public async Task Create_ShouldThrowException_WhenPhoneIsInvalid()
        {
            //Arrange
            var command = new CreateContactCommand("Ayrton", 11, "123", "ayrton.senna@example.com");

            //Act
            Func<Task> act = async () => await Sender.Send(command);

            //Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact(Skip = "WIP", DisplayName = "Should return existing contacts from database")]
        [Trait("Category", "Integration")]
        public async Task GetAll_ShouldReturnContactsList_FromDatabase()
        {
            //Arrange
            //var contact = DataSeeder.GetTestContact();

            //Act
            var result = await Sender.Send(new GetAllContactsRequest());

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1)
                .And.AllBeOfType<GetAllContactsResponse>();

            //result.Should().Contain(c => c.Equals(contact));
        }

        [Fact(Skip = "WIP", DisplayName = "Should return one contact that exists on database")]
        [Trait("Category", "Integration")]
        public async Task GetByDdd_ShouldReturnContact_WhenContactExists()
        {
            //Arrange
            var command = new CreateContactCommand("Ayrton", 13, "987654321", "ayrton.senna2@example.com");
            await Sender.Send(command);
            var query = new GetContactsByDddRequest(13);

            //Act
            var result = await Sender.Send(query);

            //Assert
            result.Should().NotBeNull()
                .And.HaveCount(3)
                .And.AllBeOfType<GetContactsByDddResponse>()
                .And.Contain(contact => contact.Name == "Ayrton" && contact.DDDCode == 13);
        }

        [Fact(DisplayName = "Should return empty list of contact from the database")]
        [Trait("Category", "Integration")]
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
