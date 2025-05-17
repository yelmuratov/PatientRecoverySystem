using PatientRecoverySystem.Domain.Entities;

namespace PatientRecoverySystem.Domain.Interfaces;

public interface IRehabilitationRepository
{
    Task AddAsync(RehabilitationRecord record);
    Task<bool> UpdateProgressAsync(int id, string progressNote);
    Task<IEnumerable<RehabilitationRecord>> GetByPatientIdAsync(int patientId);
    Task<RehabilitationRecord?> GetByIdAsync(int id);
    Task<bool> UpdatePlanAsync(int id, string plan);
    Task<bool> DeleteAsync(int id);                  
}
