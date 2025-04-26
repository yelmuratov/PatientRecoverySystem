using Microsoft.EntityFrameworkCore;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Interfaces;
using PatientRecoverySystem.Infrastructure.Data;

namespace PatientRecoverySystem.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _context.Doctors
                .Include(d => d.Patients)
                .ToListAsync();
        }

        public async Task<Doctor> GetByIdAsync(int id)
        {
            return await _context.Doctors
                .Include(d => d.Patients)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Doctor> AddAsync(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }
    }
}
