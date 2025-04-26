using PatientRecoverySystem.Application.DTOs;

namespace PatientRecoverySystem.Application.Interfaces
{
    public interface IRecoveryLogService
    {
        Task AddRecoveryLogAsync(int patientId, RecoveryLogDto logDto);
        Task<List<RecoveryLogDto>> GetRecoveryLogsByPatientIdAsync(int patientId);
        Task<RecoveryLogDto?> GetRecoveryLogByIdAsync(int id);
        Task<RecoveryLogDto?> UpdateRecoveryLogAsync(int id, RecoveryLogDto logDto);
        Task DeleteRecoveryLogAsync(int id);
        Task<List<RecoveryLogDto>> GetAllRecoveryLogsAsync();
        Task<List<RecoveryLogDto>> GetRecoveryLogsByDoctorIdAsync(int doctorId);
    }
}
