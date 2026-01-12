using HCAMiniEHR.Models;
using Microsoft.EntityFrameworkCore;

namespace HCAMiniEHR.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tables
        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<LabOrder> LabOrders { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ DEFAULT SCHEMA
            modelBuilder.HasDefaultSchema("Healthcare");
            modelBuilder.Entity<Doctor>()
    .ToTable("Doctors", "Healthcare");

            // Patient → Appointments (One-to-Many)
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Appointment → LabOrders (One-to-Many)
            modelBuilder.Entity<Appointment>()
                .HasMany(a => a.LabOrders)
                .WithOne(l => l.Appointment)
                .HasForeignKey(l => l.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // AuditLog (Independent table)
            modelBuilder.Entity<AuditLog>()
                .HasKey(a => a.AuditLogId);
        }
    }
}
