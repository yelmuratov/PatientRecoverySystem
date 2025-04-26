using PatientRecoverySystem.Domain.Entities;

namespace PatientRecoverySystem.Domain.Interfaces;

public interface IPatientRepository
{
    Task<List<Patient>> GetAllAsync();
    Task<Patient> GetByIdAsync(int id);
    // get patients by doctor id
    Task<List<Patient>> GetByDoctorIdAsync(int doctorId);
    Task<Patient> AddAsync(Patient patient);
    Task<Patient> UpdateAsync(Patient patient);
    Task DeleteAsync(int id);
    Task AddRecoveryLogAsync(RecoveryLog log);
}
