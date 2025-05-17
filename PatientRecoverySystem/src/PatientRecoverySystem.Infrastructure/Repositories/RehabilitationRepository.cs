using Microsoft.EntityFrameworkCore;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Interfaces;
using PatientRecoverySystem.Infrastructure.Data;

namespace PatientRecoverySystem.Infrastructure.Repositories
{
    public class RehabilitationRepository : IRehabilitationRepository
    {
        private readonly ApplicationDbContext _context;

        public RehabilitationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RehabilitationRecord record)
        {
            await _context.RehabilitationRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateProgressAsync(int id, string progressNote)
        {
            var record = await _context.RehabilitationRecords.FindAsync(id);
            if (record == null)
                return false;

            record.ProgressNote = progressNote;
            record.DateUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<RehabilitationRecord>> GetByPatientIdAsync(int patientId)
        {
            return await _context.RehabilitationRecords
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.DateAssigned)
                .ToListAsync();
        }

        public async Task<RehabilitationRecord?> GetByIdAsync(int id)
        {
            return await _context.RehabilitationRecords.FindAsync(id);
        }

        public async Task<bool> UpdatePlanAsync(int id, string plan)
        {
            var record = await _context.RehabilitationRecords.FindAsync(id);
            if (record == null)
                return false;

            record.Plan = plan;
            record.DateUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var record = await _context.RehabilitationRecords.FindAsync(id);
            if (record == null)
                return false;

            _context.RehabilitationRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
