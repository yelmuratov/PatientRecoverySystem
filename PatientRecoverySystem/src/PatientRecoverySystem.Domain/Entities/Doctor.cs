namespace PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Enums;

public class Doctor
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; } 

    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
