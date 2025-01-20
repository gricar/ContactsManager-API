using Bogus;
using Contacts37.Domain.Entities;
using Contacts37.Persistence;

namespace Contacts37.Application.IntegrationTests.TestData;

public class TestDataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly Faker _faker;

    // Keep track of seeded entity for test reference
    private readonly List<Contact> _contacts = new();

    public TestDataSeeder(ApplicationDbContext context)
    {
        _context = context;
        _faker = new Faker();
    }

    public async Task SeedAsync()
    {
        await SeedContacts();
        await _context.SaveChangesAsync();
    }

    private async Task SeedContacts()
    {
        var contactList = Enumerable.Range(1, 5)
          .Select(_ => Contact.Create(
                _faker.Name.FirstName(),
                _faker.PickRandom(new[] { 11, 21, 31, 41 }),
                _faker.Phone.PhoneNumber("#########"),
                _faker.Internet.Email()
              ))
          .ToList();

        _contacts.AddRange(contactList);
        await _context.Contacts.AddRangeAsync(contactList);
    }

    // Helper method to get test entity
    public Contact GetTestContact(int index = 0) => _contacts[index];
}
