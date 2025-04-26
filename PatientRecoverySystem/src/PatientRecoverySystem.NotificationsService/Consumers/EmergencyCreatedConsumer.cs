using MassTransit;
using PatientRecoverySystem.Application.Events;
using PatientRecoverySystem.NotificationsService.Data;
using PatientRecoverySystem.NotificationsService.Models;

namespace PatientRecoverySystem.NotificationsService.Consumers;

public class RecoveryLogCreatedConsumer : IConsumer<RecoveryLogCreated>
{
    private readonly NotificationDbContext _dbContext;

    public RecoveryLogCreatedConsumer(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<RecoveryLogCreated> context)
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
