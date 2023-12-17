using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Entities
{
    [Table("Allergy", Schema = "Patient")]
    public class Allergy : BaseAuditableEntity<long>
    {
        [Column("allergyName")]
        public string AllergyName {  get; set; }
        [Column("allergyTypeId")]
        public long AllergyTypeId { get; set; } 
        [Column("languageId")]
        public int? LanguageId { get; set; }
        public AllergyType? AllergyType { get; set; } = null!; // Required reference navigation to principal
        public PatientAllergy? PatientAllergy { get; set; } = null!; // Required reference navigation to principal
        //public ICollection<PatientAllergy> PatientAllergys { get; } = new List<PatientAllergy>(); // Collection navigation containing dependents
    }
}
