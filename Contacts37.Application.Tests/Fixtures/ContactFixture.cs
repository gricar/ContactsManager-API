using AutoMapper;
using Bogus;
using Contacts37.Application.Usecases.Contacts.Commands.Create;
using Contacts37.Domain.Entities;

namespace Contacts37.Application.Tests.Fixtures
{
    public class ContactFixture
    {
        public IMapper Mapper { get; }
        private readonly Faker _faker;

        public ContactFixture()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CreateContactMapper>());
            Mapper = new Mapper(config);
            _faker = new Faker();
        }

        public Contact CreateValidContact()
        {
            return Contact.Create(
                _faker.Person.FirstName,
                _faker.PickRandom(new[] { 11, 21, 31, 41 }),
                _faker.Phone.PhoneNumber("#########"),
                _faker.Person.Email);
        }

        public CreateContactCommand CreateContactCommandFromEntity(Contact contact)
        {
            return new CreateContactCommand(
                contact.Name,
                contact.Region.DddCode,
                contact.Phone,
                contact.Email);
        }

        public CreateContactCommand CreateValidContactCommand()
        {
            return new CreateContactCommand(
                _faker.Person.FirstName,
                _faker.PickRandom(new[] { 11, 21, 31, 41 }),
                _faker.Phone.PhoneNumber("#########"),
                _faker.Person.Email);
        }

        public CreateContactCommand CreateValidContactCommandWithEmailNull()
        {
            return new CreateContactCommand(
                _faker.Person.FirstName,
                _faker.PickRandom(new[] { 11, 21, 31, 41 }),
                _faker.Phone.PhoneNumber("#########"));
        }

        public CreateContactCommand CreateInvalidContactCommandWithInvalidEmail()
        {
            return new CreateContactCommand(
                _faker.Person.FirstName,
                _faker.PickRandom(new[] { 11, 21, 31, 41 }),
                _faker.Phone.PhoneNumber("#########"),
                "invalid_email");
        }

        public CreateContactCommand CreateInvalidContactCommandWithInvalidName()
        {
            return new CreateContactCommand(
                " ",
                _faker.PickRandom(new[] { 11, 21, 31, 41 }),
                _faker.Phone.PhoneNumber("#########"),
                _faker.Person.Email);
        }

        public CreateContactCommand CreateInvalidContactCommandWithInvalidPhone()
        {
            return new CreateContactCommand(
                _faker.Person.FirstName,
                _faker.PickRandom(new[] { 11, 21, 31, 41 }),
                _faker.Phone.PhoneNumber("###"),
                _faker.Person.Email);
        }
    }
}
