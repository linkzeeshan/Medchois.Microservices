using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Entities
{
    [Table("Prescription", Schema = "Patient")]
    public class Prescription : BaseAuditableEntity<long>
    {
        [Column("doctorId")]
        public long? DoctorId { get; set; }
        [Column("patientId")]
        public long? PatientId { get; set; }
        [Column("medicationName")]
        public string? MedicationName { get; set; }
        [Column("dosage")]
        public string? Dosage { get; set; }
        [Column("instructions")]
        public string? Instructions { get; set; } //discuss to update in table we will save json in it
        [Column("DatePrescribed")]
        public DateTime? DatePrescribed { get; set; }
        public Patient? Patient { get; set; } = null!; // Required reference navigation to principal
    }
}
