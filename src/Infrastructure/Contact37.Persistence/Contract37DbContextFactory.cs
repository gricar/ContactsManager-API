using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Contact37.Persistence
{
	public class Contract37DbContextFactory : IDesignTimeDbContextFactory<Contact37DbContext>
	{
		//#nota: Para fazer o Migration funcionar usando essa abordagem de Injeção de Dependência
		public Contact37DbContext CreateDbContext(string[] args)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();
			var builder = new DbContextOptionsBuilder<Contact37DbContext>();
			var connectionstring = configuration.GetConnectionString("Contact37ConnectionString");

			builder.UseSqlServer(connectionstring);

			return new Contact37DbContext(builder.Options);
		}
	}
}
