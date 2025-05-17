namespace PatientRecoverySystem.Domain.Entities;

public class RehabilitationRecord
{
    public int Id { get; set; }

    public int PatientId { get; set; }
    public int DoctorId { get; set; }

    public string Plan { get; set; } // The rehab instruction/goal
    public string? ProgressNote { get; set; } 
    
    public DateTime DateAssigned { get; set; }
    public DateTime? DateUpdated { get; set; }
}


