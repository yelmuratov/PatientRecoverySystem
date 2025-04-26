using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Enums;
using PatientRecoverySystem.Domain.Interfaces;
using PatientRecoverySystem.Infrastructure.Data;

namespace PatientRecoverySystem.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Patient> _passwordHasher;
        private readonly ApplicationDbContext _context; 

        public PatientService(
            IPatientRepository patientRepository,
            IMapper mapper,
            IPasswordHasher<Patient> passwordHasher,
            ApplicationDbContext context) // Inject DbContext
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        public async Task<List<PatientDto>> GetAllPatientsAsync(ClaimsPrincipal user)
        {
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = user.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userIdClaim))
            {
                return new List<PatientDto>();
            }

            int userId = int.Parse(userIdClaim);

            var patients = await _patientRepository.GetAllAsync();

            if (role == "Doctor")
            {
                patients = patients.Where(p => p.DoctorId == userId).ToList();
            }
            else if (role == "AdminDoctor" || role == "Moderator")
            {
                // AdminDoctor and Moderator get all patients
            }
            else
            {
                patients = new List<Patient>();
            }

            return _mapper.Map<List<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<List<PatientDto>> GetPatientsByDoctorIdAsync(int doctorId)
        {
            var patients = await _patientRepository.GetByDoctorIdAsync(doctorId);
            return _mapper.Map<List<PatientDto>>(patients);
        }

        public async Task<PatientDto> CreatePatientAsync(PatientDto dto)
        {
            var patient = _mapper.Map<Patient>(dto);

            // 🔐 Force Patient role
            patient.Role = UserRole.Patient;

            // 🔐 Hash the password before saving
            patient.Password = _passwordHasher.HashPassword(patient, dto.Password);

            var created = await _patientRepository.AddAsync(patient);
            return _mapper.Map<PatientDto>(created);
        }

        public async Task AddRecoveryLogAsync(int patientId, RecoveryLogDto logDto)
        {
            var recoveryLog = _mapper.Map<RecoveryLog>(logDto);
            recoveryLog.PatientId = patientId;
            await _patientRepository.AddRecoveryLogAsync(recoveryLog);
        }

        public async Task<PatientDto?> UpdatePatientAsync(int id, PatientDto dto)
        {
            var existingPatient = await _patientRepository.GetByIdAsync(id);
            if (existingPatient == null) return null;

            // 🔥 Validate that Doctor exists
            var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == dto.DoctorId);
            if (!doctorExists)
            {
                throw new InvalidOperationException($"Doctor with Id {dto.DoctorId} not found. Cannot assign to patient.");
            }

            // Update only allowed fields
            existingPatient.FullName = dto.FullName;
            existingPatient.Email = dto.Email;
            existingPatient.Phone = dto.Phone;
            existingPatient.DateOfBirth = dto.DateOfBirth;
            existingPatient.DoctorId = dto.DoctorId;

            // 🔥 Force Role to Patient, protect against role hacking
            existingPatient.Role = UserRole.Patient;

            // 🔐 Hash password if provided
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                existingPatient.Password = _passwordHasher.HashPassword(existingPatient, dto.Password);
            }

            var updated = await _patientRepository.UpdateAsync(existingPatient);
            return _mapper.Map<PatientDto>(updated);
        }



        public async Task DeletePatientAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient != null)
            {
                await _patientRepository.DeleteAsync(id);
            }
        }
    }
}
