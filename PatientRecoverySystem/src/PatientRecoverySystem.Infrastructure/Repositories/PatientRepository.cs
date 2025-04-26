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

        public async Task<Patient> UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<RecoveryLog>> GetRecoveryLogsByPatientIdAsync(int patientId)
        {
            return await _context.RecoveryLogs
                .Where(log => log.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<RecoveryLog> GetRecoveryLogByIdAsync(int id)
        {
            return await _context.RecoveryLogs.FindAsync(id);
        }

        public async Task<RecoveryLog> UpdateRecoveryLogAsync(RecoveryLog log)
        {
            _context.RecoveryLogs.Update(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task DeleteRecoveryLogAsync(int id)
        {
            var log = await _context.RecoveryLogs.FindAsync(id);
            if (log != null)
            {
                _context.RecoveryLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<RecoveryLog>> GetAllRecoveryLogsAsync()
        {
            return await _context.RecoveryLogs.ToListAsync();
        }

        public async Task<List<RecoveryLog>> GetRecoveryLogsByDoctorIdAsync(int doctorId)
        {
            return await _context.RecoveryLogs
                .Include(log => log.Patient)
                .Where(log => log.Patient.DoctorId == doctorId)
                .ToListAsync();
        }
    }
}
