using Contacts37.Domain.Entities;

namespace Contacts37.Application.Contracts.Persistence
{
    public interface IContactRepository : IGenericRepository<Contact>
    {
        Task<bool> IsEmailUniqueAsync(string email);
    }
}
