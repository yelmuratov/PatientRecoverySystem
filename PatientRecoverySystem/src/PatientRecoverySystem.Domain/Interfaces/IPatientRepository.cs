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

    //Recovery log methods
    Task AddRecoveryLogAsync(RecoveryLog log);
    Task<List<RecoveryLog>> GetRecoveryLogsByPatientIdAsync(int patientId);
    Task<RecoveryLog> GetRecoveryLogByIdAsync(int id);
    Task<RecoveryLog> UpdateRecoveryLogAsync(RecoveryLog log);
    Task DeleteRecoveryLogAsync(int id);
    Task<List<RecoveryLog>> GetAllRecoveryLogsAsync();
    Task<List<RecoveryLog>> GetRecoveryLogsByDoctorIdAsync(int doctorId);
}
