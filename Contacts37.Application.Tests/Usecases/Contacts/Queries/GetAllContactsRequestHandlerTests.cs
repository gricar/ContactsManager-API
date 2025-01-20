using Contacts37.Application.Contracts.Persistence;
using Contacts37.Application.Tests.Fixtures;
using Contacts37.Application.Usecases.Contacts.Queries.GetAll;
using Contacts37.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Contacts37.Application.Tests.Usecases.Contacts.Queries
{
    [Collection(nameof(ContactFixtureCollection))]
    public class GetAllContactsRequestHandlerTests
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly GetAllContactsRequestHandler _handler;
        private readonly ContactFixture _fixture;

        public GetAllContactsRequestHandlerTests(ContactFixture fixture)
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _handler = new GetAllContactsRequestHandler(_contactRepositoryMock.Object, fixture.Mapper);
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should list all contacts successfully when data exists")]
        [Trait("Category", "Get All Contacts - Success")]
        public async void GetAllContacts_ShouldSucceed_WhenContactsExist()
        {
            //Arrange
            var contacts = _fixture.CreateValidContactList();

            _contactRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(contacts);

            // Act
            var result =  await _handler.Handle(new GetAllContactsRequest(), CancellationToken.None);

            //Assert
            result.Should().BeOfType<List<GetAllContactsResponse>>();
            result.Should().HaveCount(5);
        }

        [Fact(DisplayName = "Validate get all contacts with empty list")]
        [Trait("Category", "Get All Contacts - Success")]
        public async void GetAllContacts_ShouldSucceed_WhenNoContactsExist()
        {
            //Arrange
            _contactRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Contact>());

            // Act
            var result = await _handler.Handle(new GetAllContactsRequest(), CancellationToken.None);

            //Assert
            result.Should().BeOfType<List<GetAllContactsResponse>>();
            result.Should().BeEmpty();
        }
    }
}
