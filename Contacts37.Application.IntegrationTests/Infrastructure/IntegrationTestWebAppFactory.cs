using Contacts37.Persistence;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace Contacts37.Application.IntegrationTests.Infrastructure
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer;

        public IntegrationTestWebAppFactory()
        {
            _dbContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword("StrongP@ssword123!")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove o DbContext configurado pela aplicação
                var descriptor = services
                    .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                // Registra o DbContext usando a connection string do Testcontainers
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(_dbContainer.GetConnectionString());
                });
            });
        }

        public Task InitializeAsync() => _dbContainer.StartAsync();
        public new Task DisposeAsync() => _dbContainer.StopAsync();
    }
}
