using AutoMapper;
using Bogus;
using Contacts37.Application.Usecases.Contacts.Commands.Create;
using Contacts37.Application.Usecases.Contacts.Commands.Update;
using Contacts37.Application.Usecases.Contacts.Queries.GetAll;
using Contacts37.Application.Usecases.Contacts.Queries.GetByDdd;
using Contacts37.Domain.Entities;

namespace Contacts37.Application.Tests.Fixtures
{
    public class ContactFixture
    {
        public IMapper Mapper { get; }
        private readonly Faker _faker;
        private readonly IEnumerable<int> ValidDddCodes;

        public ContactFixture()
        {
            Mapper = ConfigureMapper();
            _faker = new Faker();

            ValidDddCodes = new[] { 11, 21, 31, 41 };
        }

        private static IMapper ConfigureMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CreateContactMapper>();
                cfg.AddProfile<GetAllContactsMapper>();
                cfg.AddProfile<GetContactsByDddMapper>();
            });
            return new Mapper(config);
        }

        public Contact CreateValidContact()
        {
            return Contact.Create(
                _faker.Person.FirstName,
                _faker.PickRandom(ValidDddCodes),
                _faker.Phone.PhoneNumber("#########"),
                _faker.Person.Email);
        }

        public IEnumerable<Contact> CreateValidContactList(int count = 5)
        {
            return Enumerable.Range(1, count)
              .Select(_ => CreateValidContact())
              .ToList();
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
                _faker.PickRandom(ValidDddCodes),
                _faker.Phone.PhoneNumber("#########"),
                _faker.Person.Email);
        }

        public CreateContactCommand CreateContactCommandWithInvalidData(string? name = null, int? dddCode = null, string? phone = null, string? email = null)
        {
            return new CreateContactCommand(
                name ?? " ",
                dddCode ?? _faker.PickRandom(ValidDddCodes),
                phone ?? _faker.Phone.PhoneNumber("###"),
                email);
        }

        public CreateContactCommand CreateValidContactCommandWithEmailNull()
        {
            return new CreateContactCommand(
                _faker.Person.FirstName,
                _faker.PickRandom(ValidDddCodes),
                _faker.Phone.PhoneNumber("#########"));
        }

        public UpdateContactCommand CreateValidUpdateContactCommand(Contact contact)
        {
            return new UpdateContactCommand(
                contact.Id,
                contact.Name,
                contact.Region.DddCode,
                contact.Phone,
                contact.Email);
        }

        public UpdateContactCommand CreateValidUpdateContactCommandWithNewPhone(Contact contact)
        {
            return new UpdateContactCommand(
                contact.Id,
                contact.Name,
                _faker.PickRandom(ValidDddCodes),
                _faker.Phone.PhoneNumber("###"),
                contact.Email);
        }

        public UpdateContactCommand CreateValidUpdateContactCommandForNonExistentContact()
        {
            var contact = CreateValidContact();

            return new UpdateContactCommand(
                contact.Id,
                contact.Name,
                contact.Region.DddCode,
                contact.Phone,
                contact.Email);
        }

        public UpdateContactCommand CreateInvalidUpdateContactCommandWithInvalidEmail(Contact contact)
        {
            return new UpdateContactCommand(
                contact.Id,
                contact.Name,
                contact.Region.DddCode,
                contact.Phone,
                "invalid_email");
        }
    }
}
