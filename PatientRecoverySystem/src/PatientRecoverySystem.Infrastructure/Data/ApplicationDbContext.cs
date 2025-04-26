using Microsoft.EntityFrameworkCore;
using PatientRecoverySystem.Domain.Entities;

namespace PatientRecoverySystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<RecoveryLog> RecoveryLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Doctor entity
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.FullName).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Email).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Password).IsRequired();
            entity.Property(d => d.Role).IsRequired().HasMaxLength(20);

            // One doctor has many patients
            entity.HasMany(d => d.Patients)
                  .WithOne(p => p.Doctor)
                  .HasForeignKey(p => p.DoctorId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Patient entity
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.FullName).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Email).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Phone).HasMaxLength(20);
            entity.Property(p => p.DateOfBirth).IsRequired();

            // One patient has many recovery logs
            entity.HasMany(p => p.RecoveryLogs)
                  .WithOne(log => log.Patient)
                  .HasForeignKey(log => log.PatientId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // RecoveryLog entity
        modelBuilder.Entity<RecoveryLog>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Temperature).IsRequired();
            entity.Property(r => r.HeartRate).IsRequired();
            entity.Property(r => r.Systolic).IsRequired();
            entity.Property(r => r.Diastolic).IsRequired();
            entity.Property(r => r.PainLevel).IsRequired();
            entity.Property(r => r.Timestamp).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(r => r.IsEmergency).HasDefaultValue(false);
        });
    }
}
