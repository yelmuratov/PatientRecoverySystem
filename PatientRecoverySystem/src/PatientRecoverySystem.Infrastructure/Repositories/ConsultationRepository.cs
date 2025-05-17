using Microsoft.EntityFrameworkCore;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Interfaces;
using PatientRecoverySystem.Infrastructure.Data;

namespace PatientRecoverySystem.Infrastructure.Repositories
{
    public class ConsultationRepository : IConsultationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsultationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ConsultationRequest request)
        {
            await _context.ConsultationRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ConsultationRequest>> GetByPatientIdAsync(int patientId)
        {
            return await _context.ConsultationRequests
                .Where(x => x.PatientId == patientId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<ConsultationRequest?> GetByIdAsync(int id)
        {
            return await _context.ConsultationRequests.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
