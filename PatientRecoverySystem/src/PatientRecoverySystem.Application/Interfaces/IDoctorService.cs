using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Domain.Entities;  
using PatientRecoverySystem.Application.Parameters; 
using System.Security.Claims;

namespace PatientRecoverySystem.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<PagedResult<DoctorDto>> GetAllDoctorsAsync(DoctorQueryParameters parameters, ClaimsPrincipal user);
        Task<DoctorDto> GetDoctorByIdAsync(int id, ClaimsPrincipal user);
        Task<DoctorDto> CreateDoctorAsync(DoctorDto dto, ClaimsPrincipal user);
        Task<DoctorDto> UpdateDoctorAsync(int id, DoctorDto dto, ClaimsPrincipal user);
        Task DeleteDoctorAsync(int id, ClaimsPrincipal user);
    }
}
