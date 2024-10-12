namespace Contacts37.Domain.Interfaces
{
    public interface IContactRepository
    {
        Task<Contacts37> GetByIdAsync (Guid Id);
        Task <IEnumerable<Contacts37>> GetAllAsync ();
        Task AddAsync(Contacts37 contacts37);
        Task UpdateAsync(Contacts37 contacts37);
        Task DeleteAsync(Guid id);
    }
}