namespace Contacts37.Application.Contracts.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task<bool> ExistsAsync(Guid id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
