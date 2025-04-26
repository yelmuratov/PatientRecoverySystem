using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Interfaces;
using PatientRecoverySystem.Domain.Enums;

namespace PatientRecoverySystem.Infrastructure.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Doctor> _passwordHasher;

        public DoctorService(IDoctorRepository doctorRepository, IMapper mapper, IPasswordHasher<Doctor> passwordHasher)
        {
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int id, ClaimsPrincipal user)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            var requesterRole = user.FindFirst(ClaimTypes.Role)?.Value;

            if (doctor.Role == UserRole.AdminDoctor && requesterRole != "AdminDoctor")
            {
                throw new UnauthorizedAccessException("You are not allowed to view AdminDoctor information.");
            }

            return _mapper.Map<DoctorDto>(doctor);
        }


        public async Task<DoctorDto> CreateDoctorAsync(DoctorDto dto, ClaimsPrincipal user)
        {
            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            if (dto.Role == UserRole.AdminDoctor && userRole != "AdminDoctor")
            {
                throw new UnauthorizedAccessException("Only AdminDoctor can create another AdminDoctor.");
            }

            var doctor = _mapper.Map<Doctor>(dto);
            doctor.Password = _passwordHasher.HashPassword(doctor, dto.Password);

            var created = await _doctorRepository.AddAsync(doctor);

            return _mapper.Map<DoctorDto>(created);
        }

        public async Task<DoctorDto?> UpdateDoctorAsync(int id, DoctorDto dto, ClaimsPrincipal user)
        {
            var existingDoctor = await _doctorRepository.GetByIdAsync(id);
            if (existingDoctor == null) return null;

            var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

            if (existingDoctor.Role == UserRole.AdminDoctor && userRole != "AdminDoctor")
            {
                throw new UnauthorizedAccessException("Only AdminDoctor can update another AdminDoctor.");
            }

            existingDoctor.FullName = dto.FullName;
            existingDoctor.Email = dto.Email;
            existingDoctor.Password = dto.Password;
            existingDoctor.Role = dto.Role;

            var updatedDoctor = await _doctorRepository.UpdateAsync(id, existingDoctor);

            return _mapper.Map<DoctorDto>(updatedDoctor);
        }

        public async Task DeleteDoctorAsync(int id, ClaimsPrincipal user)
        {
            var targetDoctor = await _doctorRepository.GetByIdAsync(id);

            if (targetDoctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            var requesterRole = user.FindFirst(ClaimTypes.Role)?.Value;

            if (targetDoctor.Role == UserRole.AdminDoctor && requesterRole != "AdminDoctor")
            {
                throw new UnauthorizedAccessException("Only an AdminDoctor can delete another AdminDoctor.");
            }

            await _doctorRepository.DeleteAsync(id);
        }
    }
}
