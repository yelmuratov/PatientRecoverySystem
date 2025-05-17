using PatientRecoverySystem.Domain.Entities;

namespace PatientRecoverySystem.Domain.Interfaces
{
    public interface IConsultationRepository
    {
        Task AddAsync(ConsultationRequest request);
        Task<IEnumerable<ConsultationRequest>> GetByPatientIdAsync(int patientId);
        Task<ConsultationRequest?> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }
}
