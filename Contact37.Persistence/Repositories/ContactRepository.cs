using Contacts37.Application.Contracts.Persistence;
using Contacts37.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contacts37.Persistence.Repositories
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ContactRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Contact>> GetContactsDddCode(int dddCode)
        {
            return await _dbContext.Contacts
                                 .Where(contact => contact.Region.DddCode == dddCode)
                                 .ToListAsync();
        }

        public async Task<bool> IsDddAndPhoneUniqueAsync(int ddd, string phone)
        {
            return !await _dbContext.Contacts.AnyAsync(c =>
                            c.Region.DddCode == ddd
                            && c.Phone == phone);
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbContext.Contacts.AnyAsync(c => c.Email == email);
        }
    }
}
