using Contacts37.Domain.Common;

namespace Contacts37.Domain.Entities;

public class Contact
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }   
    public string Phone { get; set; }   
    public string DDD { get; set; }

    public Contact(string name, string phone, string email, string ddd)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Phone = phone;
        DDD = ddd;
    }

}
    
