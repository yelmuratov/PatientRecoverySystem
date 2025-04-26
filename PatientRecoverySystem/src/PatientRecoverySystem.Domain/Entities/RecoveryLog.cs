namespace PatientRecoverySystem.Domain.Entities;

public class RecoveryLog
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; } 
    public Patient Patient { get; set; }
    public double Temperature { get; set; }
    public int HeartRate { get; set; }
    public int Systolic { get; set; }
    public int Diastolic { get; set; }
    public int PainLevel { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public bool IsEmergency { get; set; } = false;

    public string Description { get; set; } = string.Empty;
}
