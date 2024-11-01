using Contacts37.Domain.Entities;
using Contacts37.Domain.Exceptions;
using FluentAssertions;

namespace Contacts37.Domain.Tests.Entities
{
    public class ContactTests
    {
        [Fact(DisplayName = "Validate contact creation with valid parameters")]
        [Trait("Category", "Create Contact - Success")]
        public void CreateContact_ValidParameters_ShouldCreate()
        {
            // Arrange
            string name = "Jony";
            int dddCode = 11;
            string phone = "123456789";
            string email = "jony@example.com";

            // Act
            var contact = Contact.Create(name, dddCode, phone, email);

            // Assert
            contact.Should().NotBeNull();
            contact.Name.Should().Be(name);
            contact.Region.DddCode.Should().Be(dddCode);
            contact.Phone.Should().Be(phone);
            contact.Email.Should().Be(email);
        }

        [Theory(DisplayName = "Validate contact creation with invalid parameters")]
        [Trait("Category", "Create Contact - Failure")]
        [InlineData("", 11, "987654321", "jony@example.com", "Name is required.")]
        [InlineData(null, 11, "987654321", "jony@example.com", "Name is required.")]
        [InlineData("Jony", 11, null, "jony@example.com", "Phone '' must be a 9-digit number.")]
        [InlineData("Jony", 11, "12345", "jony@example.com", "Phone '12345' must be a 9-digit number.")]
        [InlineData("Jony", 11, "1a3b56789", "jony@example.com", "Phone '1a3b56789' must be a 9-digit number.")]
        [InlineData("Jony", 11, "987654321", "", "Email '' must be a valid format.")]
        [InlineData("Jony", 11, "987654321", "jony@invalid", "Email 'jony@invalid' must be a valid format.")]
        [InlineData("Jony", 11, "987654321", "jony.com", "Email 'jony.com' must be a valid format.")]

        public void CreateContact_InvalidParameters_ShouldThrowException(
            string name, int dddCode, string phone, string email, string expectedErrorMessage)
        {
            // Act
            Action act = () => Contact.Create(name, dddCode, phone, email);

            // Assert
            act.Should().Throw<DomainException>()
                .WithMessage(expectedErrorMessage);
        }

        [Fact(DisplayName = "Validate updating name with valid name")]
        [Trait("Category", "Update Contact Name - Success")]
        public void UpdateContact_ValidName_ShouldUpdateName()
        {
            // Arrange
            var contact = Contact.Create("Jony", 11, "123456789", "jony@example.com");
            string newName = "Jane";

            // Act
            contact.UpdateName(newName);

            // Assert
            contact.Name.Should().Be(newName);
        }

        [Theory(DisplayName = "Validate updating name with invalid names")]
        [Trait("Category", "Update Contact Name - Failure")]
        [InlineData(null)]
        [InlineData("")]
        public void UpdateContact_InvalidName_ShouldThrowException(string invalidName)
        {
            // Arrange
            var contact = Contact.Create("Jony", 11, "123456789", "jony@example.com");

            // Act
            Action act = () => contact.UpdateName(invalidName);

            // Assert
            act.Should().Throw<InvalidNameException>()
                .WithMessage("Name is required.");
        }

        [Fact(DisplayName = "Validate updating phone with valid phone")]
        [Trait("Category", "Update Contact Phone - Success")]
        public void UpdateContact_ValidPhone_ShouldUpdatePhone()
        {
            // Arrange
            var contact = Contact.Create("Jony", 11, "123456789", "jony@example.com");
            string newPhone = "987654321";

            // Act
            contact.UpdatePhone(newPhone);

            // Assert
            contact.Phone.Should().Be(newPhone);
        }

        [Theory(DisplayName = "Validate updating phone with invalid phones")]
        [Trait("Category", "Update Contact Phone - Failure")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("12345678910")]
        [InlineData("1a3456b89")]
        public void UpdateContact_InvalidPhones_ShouldThrowException(string invalidPhone)
        {
            // Arrange
            var contact = Contact.Create("Jony", 11, "123456789", "jony@example.com");

            // Act
            Action act = () => contact.UpdatePhone(invalidPhone);

            // Assert
            act.Should().Throw<InvalidPhoneNumberException>()
                .WithMessage($"Phone '{invalidPhone}' must be a 9-digit number.");
        }

        [Fact(DisplayName = "Validate updating email with valid email")]
        [Trait("Category", "Update Contact Email - Success")]
        public void UpdateContact_ValidEmail_ShouldUpdateEmail()
        {
            // Arrange
            var contact = Contact.Create("Jony", 11, "123456789", null);
            string newEmail = "jony@example.com";

            // Act
            contact.UpdateEmail(newEmail);

            // Assert
            contact.Email.Should().Be(newEmail);
            contact.Email.Should().NotBeNull();
        }
    }
}
