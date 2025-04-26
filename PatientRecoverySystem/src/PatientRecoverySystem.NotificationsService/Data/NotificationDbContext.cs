using Microsoft.EntityFrameworkCore;
using PatientRecoverySystem.NotificationsService.Models;

namespace PatientRecoverySystem.NotificationsService.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
            : base(options)
        {
        }

        public DbSet<NotificationLog> NotificationLogs { get; set; }
    }
}
