namespace PatientRecoverySystem.Application.DTOs
{
    public class ConsultationResponseDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string SymptomDescription { get; set; } = null!;
        public string? DoctorReply { get; set; }
        public string SystemAdvice { get; set; } = null!;
        public bool EscalatedToDoctor { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
