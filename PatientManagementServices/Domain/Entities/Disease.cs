using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Entities
{
    [Table("Disease", Schema = "Patient")]
    public class Disease : BaseAuditableEntity<long>
    {
        [Column("diseaseName")]
        public string? DiseaseName { get; set; } = string.Empty;
        [Column("languageId")]
        public int? LanguageId { get; set; } = 1;
        public ICollection<PatientDisease> PatientDiseases { get; } = new List<PatientDisease>(); // Collection navigation containing dependents
    }
}
