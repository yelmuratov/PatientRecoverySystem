namespace PatientRecoverySystem.Application.Events
{
    public class EmergencyCreatedEvent
    {
        public int PatientId { get; set; }
        public string EmergencyType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
