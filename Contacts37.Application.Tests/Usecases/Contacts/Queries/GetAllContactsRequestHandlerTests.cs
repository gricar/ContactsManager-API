using AutoMapper;
using Bogus;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Application.Usecases.Contacts.Queries.GetAll;
using Contacts37.Domain.Entities;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts37.Application.Tests.Usecases.Contacts.Queries
{
    public class GetAllContactsRequestHandlerTests
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly Faker _faker;
        private readonly IMapper _mapper;

        public GetAllContactsRequestHandlerTests()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _faker = new Faker();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GetAllContactsMapper>());
            _mapper = config.CreateMapper();
        }

        [Fact(DisplayName = "Validate get all contacts")]
        [Trait("Category", "Get All Contacts - Success")]
        public async void GetAllContacts_ShouldSucceed_WhenContactsExist()
        {
            //Arrange
            string name = _faker.Person.FirstName;
            int dddCode = _faker.PickRandom(new[] { 11, 21, 31, 41 });
            string phone = _faker.Phone.PhoneNumber("#########");
            string email = _faker.Internet.Email();

            var contacts = new List<Contact>
            {
                Contact.Create(name, dddCode, phone, email)
            };

            _contactRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(contacts);

            var handler = new GetAllContactsRequestHandler(_contactRepositoryMock.Object, _mapper);

            // Act
            var result =  await handler.Handle(new GetAllContactsRequest(), CancellationToken.None);

            //Assert
            result.Should().BeOfType<List<GetAllContactsResponse>>();
            result.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Validate get all contacts with empty list")]
        [Trait("Category", "Get All Contacts - Success")]
        public async void GetAllContacts_ShouldSucceed_WhenNoContactsExist()
        {
            //Arrange
            _contactRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Contact>());

            var handler = new GetAllContactsRequestHandler(_contactRepositoryMock.Object, _mapper);

            // Act
            var result = await handler.Handle(new GetAllContactsRequest(), CancellationToken.None);

            //Assert
            result.Should().BeOfType<List<GetAllContactsResponse>>();
            result.Should().BeEmpty();
        }
    }
}
