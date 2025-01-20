using Contacts37.Application.Contracts.Persistence;
using Contacts37.Application.Tests.Fixtures;
using Contacts37.Application.Usecases.Contacts.Queries.GetByDdd;
using Contacts37.Domain.Entities;
using FluentAssertions;
using Moq;

namespace Contacts37.Application.Tests.Usecases.Contacts.Queries
{
    [Collection(nameof(ContactFixtureCollection))]
    public class GetAllContactsByDddRequestHandlerTests
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly GetContactsByDddRequestHandler _handler;
        private readonly ContactFixture _fixture;

        public GetAllContactsByDddRequestHandlerTests(ContactFixture fixture)
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _handler = new GetContactsByDddRequestHandler(_contactRepositoryMock.Object, fixture.Mapper);
            _fixture = fixture;
        }

        [Fact(DisplayName = "Should list all contacts filtered by DDD successfully when data exists")]
        [Trait("Category", "Get All Contacts by DDD - Success")]
        public async void GetAllContactsByDdd_ShouldSucceed_WhenContactsExist()
        {
            //Arrange
            var selectedDdd = 11;

            var contacts = _fixture.CreateValidContactList().Select(c =>
            {
                c.UpdateRegion(11);
                return c;
            }).ToList();

            _contactRepositoryMock.Setup(repo => repo.GetContactsDddCode(selectedDdd))
                .ReturnsAsync(contacts);

            // Act
            var result = await _handler.Handle(new GetContactsByDddRequest(selectedDdd), CancellationToken.None);

            //Assert
            result.Should().BeOfType<List<GetContactsByDddResponse>>();
            result.Should().HaveCount(contacts.Count);
        }

        [Fact(DisplayName = "Should return an empty list of contacts filtered by unexisting DDD")]
        [Trait("Category", "Get All Contacts by DDD - Success")]
        public async void GetAllContactsByDdd_ShouldSucceed_WhenNoContactsExist()
        {
            //Arrange
            var selectedDdd = 23;

            _contactRepositoryMock.Setup(repo => repo.GetContactsDddCode(selectedDdd))
                .ReturnsAsync(new List<Contact>());

            // Act
            var result = await _handler.Handle(new GetContactsByDddRequest(selectedDdd), CancellationToken.None);

            //Assert
            result.Should().BeOfType<List<GetContactsByDddResponse>>();
            result.Should().BeEmpty();
        }
    }
}
