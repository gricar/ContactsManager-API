using Contacts37.API.Middlewares;
using Contacts37.Application.DependencyInjection;
using Contacts37.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
        .ConfigurePersistenceServices(builder.Configuration)
        .ConfigureApplicationServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
