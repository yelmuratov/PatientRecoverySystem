using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Application.Parameters;
using System.Security.Claims;

namespace PatientRecoverySystem.Application.Interfaces
{
    public interface IPatientService
    {
        Task<PagedResult<PatientDto>> GetAllPatientsAsync(PatientQueryParameters parameters, ClaimsPrincipal user);
        Task<PatientDto> GetPatientByIdAsync(int id);
        Task<List<PatientDto>> GetPatientsByDoctorIdAsync(int doctorId);
        Task<PatientDto> CreatePatientAsync(PatientDto dto);
        Task<PatientDto?> UpdatePatientAsync(int id, PatientDto dto);
        Task DeletePatientAsync(int id);
    }
}
