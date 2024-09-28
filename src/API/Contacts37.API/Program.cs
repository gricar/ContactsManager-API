using Contact37.Infrastructure;
using Contact37.Persistence;
using Contacts37.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// #nota: Aqui são adicionados os ConfigureServices das outras camadas
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);
builder.Services.ConfigurePersistenceServices(builder.Configuration);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// #nota: Aqui é definida a política de permissão de acesso externo (ex:IP) à API. Está liberando tudo
builder.Services.AddCors(o =>
{
	o.AddPolicy("CorsPolicy",
		builder => builder.AllowAnyOrigin()
		.AllowAnyMethod()
		.AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//#dica: Aqui define que irá usar controle de acesso (usuario/perfil) à API
app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();
