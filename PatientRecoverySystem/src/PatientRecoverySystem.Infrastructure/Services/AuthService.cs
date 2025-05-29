using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Application.Interfaces;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PatientRecoverySystem.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<Doctor> _doctorHasher;
        private readonly IPasswordHasher<Patient> _patientHasher;

        public AuthService(
            ApplicationDbContext context,
            IConfiguration configuration,
            IPasswordHasher<Doctor> doctorHasher,
            IPasswordHasher<Patient> patientHasher)
        {
            _context = context;
            _configuration = configuration;
            _doctorHasher = doctorHasher;
            _patientHasher = patientHasher;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            // 🔐 Try Doctor
            var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (doctor != null)
            {
                var result = _doctorHasher.VerifyHashedPassword(doctor, doctor.Password, loginDto.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    return GenerateJwtTokenForDoctor(doctor);
                }
            }

            // 🔐 Try Patient
            var patient = await _context.Patients.FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (patient != null)
            {
                var result = _patientHasher.VerifyHashedPassword(patient, patient.Password, loginDto.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    return GenerateJwtTokenForPatient(patient);
                }
            }

            return null;
        }

        private string GenerateJwtTokenForDoctor(Doctor doctor)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, doctor.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, doctor.Email),
        new Claim(ClaimTypes.Role, doctor.Role.ToString()),
        new Claim(ClaimTypes.NameIdentifier, doctor.Id.ToString()),
        new Claim("FullName", doctor.FullName ?? "")
    };

            return CreateJwt(claims);
        }

        private string GenerateJwtTokenForPatient(Patient patient)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, patient.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, patient.Email),
        new Claim(ClaimTypes.Role, patient.Role.ToString()),
        new Claim(ClaimTypes.NameIdentifier, patient.Id.ToString()),
        new Claim("FullName", patient.FullName ?? ""),
        new Claim("Phone", patient.Phone ?? ""),
        new Claim("DateOfBirth", patient.DateOfBirth.ToShortDateString()),
        new Claim("Doctor", patient.DoctorId.ToString())
    };

            return CreateJwt(claims);
        }

        private string CreateJwt(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
