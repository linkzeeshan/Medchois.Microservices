using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Entities
{
    [Table("PatientDisease", Schema = "Patient")]
    public class PatientDisease : BaseAuditableEntity<long>
    {
        [Column("patientId")]
        public long? PatientId { get; set;}
        [Column("medicalRecordId")]
        public long? medicalRecordId { get; set; }
        [Column("diseaseId")]
        public long? DiseaseId { get; set; }
        [Column("customDiseaseName")]
        public string? CustomDiseaseName { get; set; } //discuss to update in table we will save json in it
        [Column("isCustomAdded")]
        public bool? IsCustomAdded { get; set; }
        public Disease? Disease { get; set; } = null!; // Required reference navigation to principal
       
    }
}
