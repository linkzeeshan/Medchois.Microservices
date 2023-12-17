using Microsoft.Extensions.Hosting;
using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Entities
{
    [Table("Operation", Schema = "Patient")]
    public class Operation : BaseAuditableEntity<long>
    {
        [Column("operationName")]
        public string? OperationName { get; set; } = string.Empty;
        [Column("operationDate")]
        public DateTime? OperationDate { get; set; } 
        [Column("languageId")]
        public int? LanguageId { get; set; }
        public ICollection<PatientOperation> PatientOperations { get; } = new List<PatientOperation>(); // Collection navigation containing dependents
    }
}
