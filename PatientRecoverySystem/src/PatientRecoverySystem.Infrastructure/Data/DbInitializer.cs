using Microsoft.AspNetCore.Identity;
using PatientRecoverySystem.Domain.Entities;
using PatientRecoverySystem.Domain.Enums;

namespace PatientRecoverySystem.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate();

            var doctorPasswordHasher = new PasswordHasher<Doctor>();
            var patientPasswordHasher = new PasswordHasher<Patient>();

            if (!context.Doctors.Any())
            {
                var doctors = new List<Doctor>
                {
                    new Doctor
                    {
                        FullName = "Admin Doctor",
                        Email = "admin@prs.com",
                        Password = doctorPasswordHasher.HashPassword(null, "123456"), // 🔥 Hash
                        Role = UserRole.AdminDoctor
                    },
                    new Doctor
                    {
                        FullName = "Normal Doctor",
                        Email = "doctor@prs.com",
                        Password = doctorPasswordHasher.HashPassword(null, "123456"), // 🔥 Hash
                        Role = UserRole.Doctor
                    },
                    new Doctor
                    {
                        FullName = "Moderator User",
                        Email = "moderator@prs.com",
                        Password = doctorPasswordHasher.HashPassword(null, "123456"), // 🔥 Hash
                        Role = UserRole.Moderator
                    }
                };

                context.Doctors.AddRange(doctors);
                context.SaveChanges();
            }

            // Seed Patients
            if (!context.Patients.Any())
            {
                var doctor = context.Doctors.FirstOrDefault(d => d.Role == UserRole.Doctor);
                if (doctor != null)
                {
                    var patient = new Patient
                    {
                        FullName = "Sample Patient",
                        Email = "patient@prs.com",
                        Password = patientPasswordHasher.HashPassword(null, "123456"), // 🔥 Hash
                        Phone = "123456789",
                        DateOfBirth = new DateTime(2000, 1, 1),
                        DoctorId = doctor.Id,
                        Role = UserRole.Patient
                    };

                    context.Patients.Add(patient);
                    context.SaveChanges();
                }
            }
        }
    }
}
