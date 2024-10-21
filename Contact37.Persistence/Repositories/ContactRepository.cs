﻿using Contacts37.Application.Contracts.Persistence;
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

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbContext.Contacts.AnyAsync(c => c.Email == email);
        }
    }
}
