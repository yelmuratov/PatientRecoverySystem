namespace PatientRecoverySystem.NotificationsService.Models
{
    public class NotificationQueryParameters
    {
        public int PageNumber { get; set; } = 1; 
        public int PageSize { get; set; } = 10;  

        public string? EmergencyType { get; set; } 
    }
}
