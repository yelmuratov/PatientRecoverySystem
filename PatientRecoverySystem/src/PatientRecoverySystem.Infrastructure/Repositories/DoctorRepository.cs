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
                .Where(d => d.Role != Domain.Enums.UserRole.AdminDoctor) // 🚀 exclude AdminDoctors
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

        public async Task<Doctor?> UpdateAsync(int id, Doctor doctor)
        {
            var existingDoctor = await _context.Doctors.FindAsync(id);

            if (existingDoctor == null)
            {
                return null; // Doctor not found
            }

            // Only update allowed fields (do NOT touch ID)
            existingDoctor.FullName = doctor.FullName;
            existingDoctor.Email = doctor.Email;
            existingDoctor.Password = doctor.Password;
            existingDoctor.Role = doctor.Role;

            await _context.SaveChangesAsync();

            return existingDoctor;
        }


        public async Task DeleteAsync(int id)
        {
            var doctor = await GetByIdAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<Doctor> Query()
        {
            return _context.Doctors.AsQueryable();
        }
    }
}
