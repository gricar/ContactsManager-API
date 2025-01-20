using Contacts37.Application.IntegrationTests.Fixtures;

namespace Contacts37.Application.IntegrationTests.Infrastructure;

[CollectionDefinition(nameof(IntegrationTestCollection))]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>, ICollectionFixture<ContactFixture>;
