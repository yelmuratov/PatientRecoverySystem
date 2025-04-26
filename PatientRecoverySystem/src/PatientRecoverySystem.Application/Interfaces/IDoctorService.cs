using PatientRecoverySystem.Application.DTOs;
using System.Security.Claims;

namespace PatientRecoverySystem.Application.Interfaces;

public interface IDoctorService
{
    Task<List<DoctorDto>> GetAllDoctorsAsync();
    Task<DoctorDto> GetDoctorByIdAsync(int id, ClaimsPrincipal user);
    Task<DoctorDto> CreateDoctorAsync(DoctorDto dto, ClaimsPrincipal user);
    Task<DoctorDto> UpdateDoctorAsync(int id, DoctorDto dto, ClaimsPrincipal user);
    Task DeleteDoctorAsync(int id, ClaimsPrincipal user);
}
