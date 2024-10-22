using Contacts37.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts37.Persistence.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.DddCode)
                .IsRequired();

            builder.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(9);

            builder.Property(c => c.Email)
                .HasMaxLength(200);

            builder.HasIndex(c => c.Email)
                .IsUnique();
        }
    }
}
