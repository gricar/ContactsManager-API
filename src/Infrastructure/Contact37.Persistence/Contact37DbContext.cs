using Contacts37.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Contact37.Persistence
{
	public class Contact37DbContext : DbContext
	{
        public Contact37DbContext(DbContextOptions<Contact37DbContext> options) : base(options)
        {
            
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(Contact37DbContext).Assembly);

			base.OnModelCreating(modelBuilder);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			foreach (var entry in ChangeTracker.Entries<BaseDomainEntity>())
			{
				entry.Entity.LastModifiedDate = DateTime.Now;
				if (entry.State == EntityState.Added)
					entry.Entity.DateCreated = DateTime.Now;
			}
			return base.SaveChangesAsync(cancellationToken);
		}
    }
}
