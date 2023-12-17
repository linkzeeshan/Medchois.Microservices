using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Entities
{
    [Table("AllergyType", Schema = "Patient")]
    public class AllergyType : BaseAuditableEntity<long>
    {
        [Column("name")]
        public string Name {  get; set; } = string.Empty;
        [Column("languageId")]
        public int? LanguageId { get; set; }
        public ICollection<Allergy>? Allergy { get; } = new List<Allergy>(); // Collection navigation containing dependents
    }
}
