using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PatientManagementServices.Domain.Entities;
using System.Reflection;

namespace PatientManagementServices.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<GenericAttribute> GenericAttributes => Set<GenericAttribute>();
        public DbSet<Allergy> Allergies => Set<Allergy>();
        public DbSet<PatientAllergy> PatientAllergies => Set<PatientAllergy>();
        public DbSet<Disease> Diseases => Set<Disease>();
        public DbSet<Feedback> Feedbacks => Set<Feedback>();
        public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
        public DbSet<Operation> Operations => Set<Operation>();
        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Operation>()
                .HasMany(o => o.PatientOperations)
                .WithOne(e => e.Operation)
                .HasForeignKey(e => e.OperationId)
                .IsRequired(false);

            builder.Entity<Disease>()
                .HasMany(o => o.PatientDiseases)
                .WithOne(e => e.Disease)
                .HasForeignKey(e => e.DiseaseId)
                .IsRequired(false);

            builder.Entity<AllergyType>()
                .HasMany(o => o.Allergy)
                .WithOne(e => e.AllergyType)
                .HasForeignKey(e => e.AllergyTypeId)
                .IsRequired(false);

            builder.Entity<Allergy>()
                .HasOne(o => o.PatientAllergy)
                .WithOne(e => e.Allergy)
                .HasForeignKey<PatientAllergy>(e => e.AllergyId)
                .IsRequired(false);

            builder.Entity<Patient>()
               .HasMany(o => o.Feedbacks)
               .WithOne(e => e.Patient)
               .HasForeignKey(e => e.PatientId)
               .IsRequired(false);

            builder.Entity<Patient>()
               .HasMany(o => o.Prescriptions)
               .WithOne(e => e.Patient)
               .HasForeignKey(e => e.PatientId)
               .IsRequired(false);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);

        }
    }
}
