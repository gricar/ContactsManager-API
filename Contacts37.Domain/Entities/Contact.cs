using Contacts37.Domain.Common;
using Contacts37.Domain.ValueObjects;

namespace Contacts37.Domain.Entities
{
    public class Contact : BaseDomainEntity
    {
        public string Name { get; private set; }
        public Region Region { get; private set; }
        public string Phone { get; private set; }
        public string? Email { get; private set; }

        public Contact(string name, int dddCode, string phone, string? email = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");
            if (string.IsNullOrWhiteSpace(phone) || !IsValidPhoneNumber(phone))
                throw new ArgumentException("Phone is required and must be a 9-digit number.");

            Name = name;
            Region = new Region(dddCode);
            Phone = phone;
            Email = email;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name is required.");
            Name = newName;
        }

        public void UpdateRegion(int newDddCode)
        {
            Region = new Region(newDddCode); // Cria uma nova instância com o novo DDD
        }

        public void UpdatePhone(string newPhone)
        {
            if (string.IsNullOrWhiteSpace(newPhone) || !IsValidPhoneNumber(newPhone))
                throw new ArgumentException("Phone must be a 9-digit number.");

            Phone = newPhone;
        }

        public void UpdateEmail(string? newEmail) => Email = newEmail;

        private static bool IsValidPhoneNumber(string phone) =>
            phone.Length == 9 && phone.All(char.IsDigit);

        // Migration updated needed
        protected Contact() { }
    }
}
