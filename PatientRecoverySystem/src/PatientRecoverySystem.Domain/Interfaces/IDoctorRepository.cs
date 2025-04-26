using PatientRecoverySystem.Domain.Entities;

namespace PatientRecoverySystem.Domain.Interfaces;

public interface IDoctorRepository
{
    Task<List<Doctor>> GetAllAsync();
    Task<Doctor> GetByIdAsync(int id);
    Task<Doctor> AddAsync(Doctor doctor);
    Task<Doctor> UpdateAsync(int id, Doctor doctor);
    Task DeleteAsync(int id);
    IQueryable<Doctor> Query();
}
