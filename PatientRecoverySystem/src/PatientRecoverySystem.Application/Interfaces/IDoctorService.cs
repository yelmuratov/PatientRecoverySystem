using PatientRecoverySystem.Application.DTOs;

namespace PatientRecoverySystem.Application.Interfaces;

public interface IDoctorService
{
    Task<List<DoctorDto>> GetAllDoctorsAsync();
    Task<DoctorDto> GetDoctorByIdAsync(int id);
    Task<DoctorDto> CreateDoctorAsync(DoctorDto dto);
}
