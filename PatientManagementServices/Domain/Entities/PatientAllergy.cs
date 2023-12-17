using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Entities
{
    [Table("PatientAllergy", Schema = "Patient")]
    public class PatientAllergy : BaseAuditableEntity<long>
    {
        [Column("patientId")]
        public long PatientId { get; set;}
        [Column("medicalRecordId")]
        public long? MedicalRecordId { get; set; }
        [Column("allergyId")]
        public long? AllergyId { get; set; }
        [Column("customAllergyName")]
        public string? CustomAllergyName { get; set; } //discuss to update in table we will save json in it
        [Column("customAllergyType")]
        public string? CustomAllergyType { get; set; }
        [Column("isCustomAdded")]
        public bool? IsCustomAdded { get; set; }
        public Allergy? Allergy { get; set; } = null!; // Required reference navigation to principal
    }
}
