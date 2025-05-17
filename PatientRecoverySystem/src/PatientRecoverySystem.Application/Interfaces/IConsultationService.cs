using PatientRecoverySystem.Application.DTOs;

namespace PatientRecoverySystem.Application.Interfaces
{
    public interface IConsultationService
    {
        Task<ConsultationResponseDto> AskConsultationAsync(int patientId, AskConsultationDto dto);
        Task<IEnumerable<ConsultationResponseDto>> GetConsultationsByPatientAsync(int patientId);
        Task<ConsultationResponseDto?> GetByIdAsync(int id);
        Task<bool> ReplyToConsultationAsync(int consultationId, ReplyConsultationDto dto);

    }
}
