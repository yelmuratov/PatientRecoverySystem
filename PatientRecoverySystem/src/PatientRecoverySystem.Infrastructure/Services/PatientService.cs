using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Enums;
using PatientRecoverySystem.Domain.Interfaces;
using PatientRecoverySystem.Infrastructure.Data;
using System.Security.Claims;
using PatientRecoverySystem.Application.Parameters;
using PatientRecoverySystem.Application.Helpers;
using PatientRecoverySystem.Application.Exceptions;

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
            ApplicationDbContext context)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        public async Task<PagedResult<PatientDto>> GetAllPatientsAsync(PatientQueryParameters parameters, ClaimsPrincipal user)
        {
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userIdClaim))
                return new PagedResult<PatientDto>(new List<PatientDto>(), 0, parameters.PageNumber, parameters.PageSize);

            int userId = int.Parse(userIdClaim);
            var query = _context.Patients.AsQueryable();

            if (role == "Doctor")
                query = query.Where(p => p.DoctorId == userId);

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                query = query.Where(p => p.FullName.Contains(parameters.Search) || p.Email.Contains(parameters.Search));
            }

            query = query.OrderByDynamic(parameters.SortBy ?? "FullName", parameters.SortDirection?.ToLower() == "desc");

            var totalItems = await query.CountAsync();

            var patients = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var mappedPatients = _mapper.Map<List<PatientDto>>(patients);

            return new PagedResult<PatientDto>(mappedPatients, totalItems, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<List<PatientDto>> GetPatientsByDoctorIdAsync(int doctorId, ClaimsPrincipal user)
        {
            var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == doctorId);
            if (!doctorExists)
                throw new NotFoundException($"Doctor with id {doctorId} not found.");

            var patients = await _patientRepository.GetByDoctorIdAsync(doctorId);
            return _mapper.Map<List<PatientDto>>(patients);
        }


        public async Task<PatientDto> CreatePatientAsync(PatientDto dto)
        {
            var patient = _mapper.Map<Patient>(dto);

            patient.Role = UserRole.Patient;
            patient.Password = _passwordHasher.HashPassword(patient, dto.Password);

            var created = await _patientRepository.AddAsync(patient);
            return _mapper.Map<PatientDto>(created);
        }

        public async Task<PatientDto?> UpdatePatientAsync(int id, PatientDto dto)
        {
            var existingPatient = await _patientRepository.GetByIdAsync(id);
            if (existingPatient == null) return null;

            var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == dto.DoctorId);
            if (!doctorExists)
                throw new InvalidOperationException($"Doctor with Id {dto.DoctorId} not found. Cannot assign to patient.");

            existingPatient.FullName = dto.FullName;
            existingPatient.Email = dto.Email;
            existingPatient.Phone = dto.Phone;
            existingPatient.DateOfBirth = dto.DateOfBirth;
            existingPatient.DoctorId = dto.DoctorId;
            existingPatient.Role = UserRole.Patient;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                existingPatient.Password = _passwordHasher.HashPassword(existingPatient, dto.Password);

            var updated = await _patientRepository.UpdateAsync(existingPatient);
            return _mapper.Map<PatientDto>(updated);
        }

        public async Task DeletePatientAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient != null)
                await _patientRepository.DeleteAsync(id);
        }
    }
}
