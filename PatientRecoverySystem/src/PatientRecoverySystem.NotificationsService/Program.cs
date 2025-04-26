using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PatientRecoverySystem.Application.Events;
using PatientRecoverySystem.NotificationsService.Data;
using PatientRecoverySystem.NotificationsService.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Configure Services
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NotificationDb")));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = Environment.GetEnvironmentVariable("RabbitMQ__Host") ?? "localhost";

        cfg.Host(rabbitMqHost, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.UseRetry(retryConfig =>
        {
            retryConfig.Interval(10, TimeSpan.FromSeconds(5)); // Try 10 times, every 5 seconds
        });
    });
});

// Add Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PatientRecoverySystem.NotificationsService",
        Version = "v1"
    });
});

var app = builder.Build(); // ✅ Must be WebApplication

// Configure Middleware
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run(); // ✅ Correct
