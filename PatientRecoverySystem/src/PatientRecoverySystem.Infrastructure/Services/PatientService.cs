using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Interfaces;
using System.Security.Claims;

namespace PatientRecoverySystem.Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Patient> _passwordHasher;

        public PatientService(
            IPatientRepository patientRepository,
            IMapper mapper,
            IPasswordHasher<Patient> passwordHasher)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
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
    }
}
