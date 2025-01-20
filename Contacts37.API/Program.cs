using Contacts37.API.Extensions;
using Contacts37.API.Middlewares;
using Contacts37.Application.DependencyInjection;
using Contacts37.Persistence.DependencyInjection;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
        .ConfigurePersistenceServices(builder.Configuration)
        .ConfigureApplicationServices();

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration)
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMetricServer();

app.UseHttpMetrics();

app.MapControllers();

app.Run();

public partial class Program { }
