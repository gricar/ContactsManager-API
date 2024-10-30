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

        private static bool IsValidPhoneNumber(string phone) =>
            phone.Length == 9 && phone.All(char.IsDigit);

        // Migration updated needed
        protected Contact() { }
    }
}
