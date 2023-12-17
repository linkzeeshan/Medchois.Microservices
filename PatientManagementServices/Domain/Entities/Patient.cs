using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace PatientManagementServices.Domain.Entities
{
    [Table("Patient", Schema = "Patient")]
    public class Patient : BaseAuditableEntity<long>
    {
        [Column("tenantId")]
        public string TenantId { get; set; }
        [Column("patientId")]
        public string patientId { get; set; }
        [Column("firstName")]
        public string? FirstName { get; set; }
        [Column("lastName")]
        public string? LastName { get; set; }
        [Column("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }
        [Column("gender")]
        public string? Gender { get; set; }
        [Column("nationalityId")]
        public string? NationalityId { get; set; }
        [Column("languageId")]
        public bool LanguageId { get; set; }
        [Column("isOnline")]
        public bool IsOnline { get; set; }
        public BodyMeasurement BodyMeasurement { get; set; } = new BodyMeasurement();
        public ICollection<Feedback> Feedbacks { get; } = new List<Feedback>(); // Collection navigation containing dependents
        public ICollection<Prescription> Prescriptions { get; } = new List<Prescription>(); // Collection navigation containing dependents
    }
}
