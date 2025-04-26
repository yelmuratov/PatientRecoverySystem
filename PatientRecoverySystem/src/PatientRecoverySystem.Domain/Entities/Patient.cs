using PatientRecoverySystem.Domain.Enums; // Add for UserRole

namespace PatientRecoverySystem.Domain.Entities
{
    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; } = UserRole.Patient; 

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public ICollection<RecoveryLog> RecoveryLogs { get; set; } = new List<RecoveryLog>();
    }
}
