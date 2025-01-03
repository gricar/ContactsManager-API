using Contacts37.API.Middlewares;
using Contacts37.Application.DependencyInjection;
using Contacts37.Persistence;
using Contacts37.Persistence.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMetricServer();

app.UseHttpMetrics();

app.MapControllers();

app.Run();

public partial class Program { }
