using PatientRecoverySystem.Application.DTOs;
using System.Security.Claims;

namespace PatientRecoverySystem.Application.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllPatientsAsync(ClaimsPrincipal user);
        Task<PatientDto> GetPatientByIdAsync(int id);
        Task<List<PatientDto>> GetPatientsByDoctorIdAsync(int doctorId);
        Task<PatientDto> CreatePatientAsync(PatientDto dto);
        Task AddRecoveryLogAsync(int patientId, RecoveryLogDto logDto);
    }
}
