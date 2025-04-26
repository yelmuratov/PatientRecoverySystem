using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PatientRecoverySystem.Application.Events;
using PatientRecoverySystem.NotificationsService.Data;
using PatientRecoverySystem.NotificationsService.Consumers;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Configure Services
// =========================

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NotificationDb")));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EmergencyCreatedConsumer >(); // 👈 ADD THIS

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = Environment.GetEnvironmentVariable("RabbitMQ__Host") ?? "localhost";

        cfg.Host(rabbitMqHost, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("recovery-log-created-event-queue", e =>
        {
            e.ConfigureConsumer<EmergencyCreatedConsumer>(context);
        });

        cfg.UseRetry(retryConfig =>
        {
            retryConfig.Interval(10, TimeSpan.FromSeconds(5));
        });
    });
});


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

var app = builder.Build();

// =========================
// Configure Middleware
// =========================

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// =========================
// Ensure Database Exists
// =========================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<NotificationDbContext>();
    dbContext.Database.Migrate(); 
}

app.Run();
