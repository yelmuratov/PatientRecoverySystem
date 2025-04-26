using MassTransit;
using PatientRecoverySystem.Application.Events;
using PatientRecoverySystem.NotificationsService.Data;
using PatientRecoverySystem.NotificationsService.Models;

namespace PatientRecoverySystem.NotificationsService.Consumers;
public class EmergencyCreatedConsumer : IConsumer<EmergencyCreatedEvent>
{
    private readonly NotificationDbContext _context;

    public EmergencyCreatedConsumer(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<EmergencyCreatedEvent> context)
    {
        var message = context.Message;

        var log = new NotificationLog
        {
            PatientId = message.PatientId,
            EmergencyType = message.EmergencyType,
            CreatedAt = message.CreatedAt,
            ReceivedAt = DateTime.UtcNow
        };

        _context.NotificationLogs.Add(log);
        await _context.SaveChangesAsync();

        Console.WriteLine($"[NotificationService] Emergency saved: PatientId={message.PatientId}, Type={message.EmergencyType}");
    }
}
