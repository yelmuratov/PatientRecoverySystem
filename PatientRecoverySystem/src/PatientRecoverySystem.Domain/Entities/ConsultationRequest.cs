namespace PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Enums;
public class ConsultationRequest
{
    public int Id { get; set; }

    public int PatientId { get; set; }
    public string SymptomDescription { get; set; }
    public string? DoctorReply { get; set; }
    public string SystemAdvice { get; set; }
    public bool EscalatedToDoctor { get; set; }

    public DateTime CreatedAt { get; set; }
}
