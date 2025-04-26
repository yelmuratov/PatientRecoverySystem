namespace PatientRecoverySystem.NotificationsService.Models
{
    public class NotificationLog
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string EmergencyType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}
