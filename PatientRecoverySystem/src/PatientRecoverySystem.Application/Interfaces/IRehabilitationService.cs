using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Domain.Entities;

namespace PatientRecoverySystem.Application.Interfaces
{
    public interface IRehabilitationService
    {
        Task AddRehabilitationPlanAsync(int patientId, CreateRehabilitationPlanDto dto);
        Task<bool> UpdateRehabilitationProgressAsync(int id, UpdateRehabilitationProgressDto dto);
        Task<bool> UpdateRehabilitationPlanAsync(int id, UpdateRehabilitationPlanDto dto);
        Task<IEnumerable<RehabilitationRecord>> GetRehabilitationProgressAsync(int patientId);
        Task<bool> DeleteRehabilitationPlanAsync(int id);
    }

}
