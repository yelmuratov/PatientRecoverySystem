namespace PatientRecoverySystem.Application.DTOs;

public class RecoveryLogDto
{
    public double Temperature { get; set; }
    public int HeartRate { get; set; }
    public int Systolic { get; set; }
    public int Diastolic { get; set; }
    public int PainLevel { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
