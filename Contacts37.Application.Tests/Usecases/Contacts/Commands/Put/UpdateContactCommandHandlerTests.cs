using AutoMapper;
using AutoMapper.Configuration;
using Bogus;
using Bogus.DataSets;
using Contacts37.Application.Common.Exceptions;
using Contacts37.Application.Contracts.Persistence;
using Contacts37.Application.Usecases.Contacts.Commands.Update;
using Contacts37.Domain.Entities;
using Contacts37.Domain.Exceptions;
using Contacts37.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Contacts37.Application.Tests.Usecases.Contacts.Commands.Put
{
    public class UpdateContactCommandHandlerTests
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly UpdateContactCommandHandler _handler;
        private readonly Faker _faker;
        public UpdateContactCommandHandlerTests()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _handler = new UpdateContactCommandHandler(_contactRepositoryMock.Object);
            _faker = new Faker();

        }

        [Fact(DisplayName = "Validate contact update when contact exists")]
        [Trait("Category", "Update Contact - Sucess")]
        public async void UpdateContact_ShouldSucess_WhenContactExists()
        {
            // Arrange
            string name = _faker.Person.FirstName;
            int dddCode = _faker.PickRandom(new[] { 11, 21, 31, 41 });
            string phone = _faker.Phone.PhoneNumber("#########");
            string email = _faker.Internet.Email();

            var contact = Contact.Create(name, dddCode, phone, email);

            _contactRepositoryMock.Setup(repo => repo.GetAsync(contact.Id))
                .ReturnsAsync(contact);

            _contactRepositoryMock.Setup(repo => repo.AddAsync(contact));

            var command = new UpdateContactCommand(contact.Id, name, dddCode, phone, email);
            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();
            _contactRepositoryMock.Verify(repo => repo.GetAsync(contact.Id), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.UpdateAsync(contact), Times.Once);
        }

        [Fact(DisplayName = "Validate contact update when invalid email")]
        [Trait("Category", "Update Contact - Failure - Invalid Email")]
        public async void UpdateContact_ShouldThrowException_WhenContactInvalidEmail()
        {
            // Arrange
            string name = _faker.Person.FirstName;
            int dddCode = _faker.PickRandom(new[] { 11, 21, 31, 41 });
            string phone = _faker.Phone.PhoneNumber("#########");
            string email = _faker.Internet.Email();

            var contact = Contact.Create(name, dddCode, phone, email);

            var command = new UpdateContactCommand(contact.Id, name, dddCode, phone, "email123");

            _contactRepositoryMock.Setup(repo => repo.GetAsync(contact.Id))
                .ReturnsAsync(contact);

            _contactRepositoryMock.Setup(repo => repo.IsEmailUniqueAsync(command.Email!))
                .ReturnsAsync(true);

            _contactRepositoryMock.Setup(repo => repo.AddAsync(contact));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidEmailException>()
                .WithMessage($"Email '{command.Email!}' must be a valid format.");
            _contactRepositoryMock.Verify(repo => repo.GetAsync(contact.Id), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.UpdateAsync(contact), Times.Never);
        }

        [Fact(DisplayName = "Validate contact update when duplicated fone")]
        [Trait("Category", "Update Contact - Failure - Duplicated Fone")]
        public async void UpdateContact_ShouldThrowException_WhenContactDuplicatedFone()
        {
            // Arrange
            string name = _faker.Person.FirstName;
            int dddCode = _faker.PickRandom(new[] { 11, 21, 31, 41 });
            string phone = _faker.Phone.PhoneNumber("#########");
            string email = _faker.Internet.Email();

            var contact1 = Contact.Create(name, dddCode, phone, email);
            _contactRepositoryMock.Setup(repo => repo.GetAsync(contact1.Id))
                .ReturnsAsync(contact1);
            _contactRepositoryMock.Setup(repo => repo.AddAsync(contact1));

            name = _faker.Person.FirstName;
            email = _faker.Internet.Email();
            dddCode = _faker.PickRandom(new[] { 11, 21, 31, 41 });
            phone = _faker.Phone.PhoneNumber("#########");
            var contact2 = Contact.Create(name, dddCode, phone, email);
            _contactRepositoryMock.Setup(repo => repo.GetAsync(contact2.Id))
                .ReturnsAsync(contact2);
            _contactRepositoryMock.Setup(repo => repo.AddAsync(contact2));

            var command = new UpdateContactCommand(contact2.Id, name, contact1.Region.DddCode, contact1.Phone, email);
            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateContactException>()
                .WithMessage($"A contact with the same DDD '{contact1.Region.DddCode}' and phone '{contact1.Phone}' already exists.");
            _contactRepositoryMock.Verify(repo => repo.GetAsync(contact2.Id), Times.Once);
            _contactRepositoryMock.Verify(repo => repo.UpdateAsync(contact2), Times.Never);
        }
    }
}
