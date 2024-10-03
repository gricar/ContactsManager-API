using Contacts37.Application.Common;

namespace Contacts37.Application.DTOs
{
    public class ContactDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DDD { get; set; }
    }

}