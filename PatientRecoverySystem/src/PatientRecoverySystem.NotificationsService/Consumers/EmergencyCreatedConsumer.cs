using MassTransit;
using PatientRecoverySystem.Application.Events;
using PatientRecoverySystem.NotificationsService.Data;
using PatientRecoverySystem.NotificationsService.Models;

namespace PatientRecoverySystem.NotificationsService.Consumers;

public class EmergencyCreatedConsumer : IConsumer<EmergencyCreatedEvent>
{
    private readonly NotificationDbContext _dbContext;

    public EmergencyCreatedConsumer(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<EmergencyCreatedEvent> context)
    {
        var message = context.Message;

        var notification = new NotificationLog
        {
            PatientId = message.PatientId,
            EmergencyType = message.EmergencyType,
            CreatedAt = DateTime.UtcNow,
            ReceivedAt = DateTime.UtcNow
        };

        _dbContext.NotificationLogs.Add(notification);
        await _dbContext.SaveChangesAsync();
    }
}
