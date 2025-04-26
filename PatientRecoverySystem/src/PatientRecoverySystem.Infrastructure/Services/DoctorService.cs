using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Interfaces;

namespace PatientRecoverySystem.Infrastructure.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Doctor> _passwordHasher;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IMapper mapper,
            IPasswordHasher<Doctor> passwordHasher)
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

        public async Task<DoctorDto> GetDoctorByIdAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto> CreateDoctorAsync(DoctorDto dto)
        {
            var doctor = _mapper.Map<Doctor>(dto);

            // 🔐 Hash the password before saving
            doctor.Password = _passwordHasher.HashPassword(doctor, dto.Password);

            var created = await _doctorRepository.AddAsync(doctor);

            return _mapper.Map<DoctorDto>(created);
        }
    }
}
