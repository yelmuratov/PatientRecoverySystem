using Microsoft.EntityFrameworkCore;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Interfaces;
using PatientRecoverySystem.Infrastructure.Data;

namespace PatientRecoverySystem.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.RecoveryLogs)
                .ToListAsync();
        }

        public async Task<Patient> GetByIdAsync(int id)
        {
            return await _context.Patients
                .Include(p => p.RecoveryLogs)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Patient>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.Patients
                .Include(p => p.RecoveryLogs)
                .Where(p => p.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<Patient> AddAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task AddRecoveryLogAsync(RecoveryLog log)
        {
            _context.RecoveryLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
