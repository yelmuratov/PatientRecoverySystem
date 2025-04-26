namespace PatientRecoverySystem.Application.DTOs;

public class RecoveryLogDto
{
    public int Id { get; set; }
    public double Temperature { get; set; }
    public int HeartRate { get; set; }
    public int Systolic { get; set; }
    public int Diastolic { get; set; }
    public int PainLevel { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = string.Empty;
    public bool IsEmergency { get; set; } = false;

    public int PatientId { get; set; } // Foreign key to Patient entity
    public int DoctorId { get; set; } // Foreign key to Doctor entity
}
