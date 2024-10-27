using Contacts37.Domain.Entities;

namespace Contacts37.Application.Contracts.Persistence
{
    public interface IContactRepository : IGenericRepository<Contact>
    {
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsDddAndPhoneUniqueAsync(int ddd, string phone);
        Task<IEnumerable<Contact>> GetContactsDddCode(int dddCode);
    }
}
