using AutoMapper;
using Bogus;
using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Application.Usecases.Contacts.Commands.Create;
using Contacts37.Domain.Entities;
using Contacts37.Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace Contacts37.Application.Tests.Usecases.Contacts.Commands.Create
{
    public class CreateContactCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly Faker _faker;
        public CreateContactCommandHandlerTests()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _faker = new Faker();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<CreateContactMapper>());
            _mapper = new Mapper(config);

        }

        [Fact(DisplayName = "Validate create contact")]
        [Trait("Category", "Create Contact - Sucess")]
        public async void CreateContact_ShouldSucess_WhenDataIsValidAndUnique()
        {
            // Arrange            
            string name = _faker.Person.FirstName;
            int dddCode = _faker.PickRandom(new[] { 11, 21, 31, 41 });
            string phone = _faker.Phone.PhoneNumber("#########");
            string email = _faker.Internet.Email();

            var command = new CreateContactCommand(name, dddCode, phone, email);

            _contactRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(command.Email!))
                .ReturnsAsync(true);

            _contactRepositoryMock.Setup(repo => repo.IsDddAndPhoneUniqueAsync(command.DDDCode , command.Phone))
                .ReturnsAsync(true);

            var handler = new CreateContactCommandHandler(_contactRepositoryMock.Object, _mapper);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreateContactCommandResponse>();
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Once);
        }

        [Fact(DisplayName = "Validate contact when invalid name")]
        [Trait("Category", "Create Contact - Failure - Invalid Name")]
        public async void CreateContact_ShouldThrowException_WhenContactInvalidName()
        {
            // Arrange
            string name = " ";
            int dddCode = _faker.PickRandom(new[] { 11, 21, 31, 41 });
            string phone = _faker.Phone.PhoneNumber("#########");
            string email = _faker.Internet.Email();

            var contact = Contact.Create(name, dddCode, phone, email);

            var handler = new CreateContactCommandHandler(_contactRepositoryMock.Object, _mapper);

            var command = new CreateContactCommand(name, dddCode, phone, email);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidNameException>()
                .WithMessage($"Name is required.");
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Once);
        }

        [Fact(DisplayName = "Validate contact when invalid fone")]
        [Trait("Category", "Create Contact - Failure - Invalid Fone")]
        public async void CreateContact_ShouldThrowException_WhenContactInvalidFone()
        {
            // Arrange
            string name = _faker.Person.FirstName;
            int dddCode = _faker.PickRandom(new[] { 11, 21, 31, 41 });
            string phone = _faker.Phone.PhoneNumber("###");
            string email = _faker.Internet.Email();

            var contact = Contact.Create(name, dddCode, phone, email);

            var handler = new CreateContactCommandHandler(_contactRepositoryMock.Object, _mapper);

            var command = new CreateContactCommand(name, dddCode, phone, email);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidPhoneNumberException>()
                .WithMessage($"Phone '{phone}' must be a 9-digit number.");
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Once);
        }

        [Fact(DisplayName = "Validate contact when invalid email")]
        [Trait("Category", "Create Contact - Failure - Invalid Email")]
        public async void CreateContact_ShouldThrowException_WhenContactInvalidEmail()
        {
            // Arrange
            string name = _faker.Person.FirstName;
            int dddCode = _faker.PickRandom(new[] { 11, 21, 31, 41 });
            string phone = _faker.Phone.PhoneNumber("#########");
            string email = _faker.Internet.Email();

            var contact = Contact.Create(name, dddCode, phone, email);

            var handler = new CreateContactCommandHandler(_contactRepositoryMock.Object, _mapper);

            var command = new CreateContactCommand(name, dddCode, phone, "email123");

            _contactRepositoryMock.Setup(repo => repo.GetAsync(contact.Id))
                .ReturnsAsync(contact);

            _contactRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(command.Email!))
                .ReturnsAsync(true);

            _contactRepositoryMock.Setup(repo => repo.DeleteAsync(contact))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidEmailException>()
                .WithMessage($"Email '{command.Email!}' must be a valid format.");
            _contactRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Contact>()), Times.Once);
        }
    }
}
