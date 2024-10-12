using Contacts37.Domain.Common;

namespace Contacts37.Domain.Entities
{
    public class Contact : BaseDomainEntity
    {
        public string Name { get; private set; }
        public string DddCode { get; private set; }
        public string Phone { get; private set; }
        public string? Email { get; private set; }

        public Contact(string name, string dddCode, string phone, string? email)
        {
            Name = name;
            DddCode = dddCode;
            Phone = phone;
            Email = email;
        }

        protected Contact() { }
    }
}
