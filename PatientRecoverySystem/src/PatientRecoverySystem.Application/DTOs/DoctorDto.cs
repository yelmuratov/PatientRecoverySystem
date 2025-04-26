namespace PatientRecoverySystem.Application.DTOs;
using PatientRecoverySystem.Domain.Enums;

public class DoctorDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // Plain password field
    public UserRole Role { get; set; }
}
