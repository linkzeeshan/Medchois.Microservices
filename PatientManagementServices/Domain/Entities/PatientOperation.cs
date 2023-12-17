using Microsoft.Extensions.Hosting;
using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Entities
{
    [Table("PatientOperation", Schema = "Patient")]
    public class PatientOperation : BaseAuditableEntity<long>
    {
        [Column("PatientId")]
        public long? PatientId { get; set; }
        [Column("medicalRecordId")]
        public long? MedicalRecordId { get; set; }
        [Column("operationId")]
        public long? OperationId { get; set; }
        [Column("customOperationName")]
        public string? CustomOperationName { get; set; } = string.Empty;
        [Column("customOperationDate")]
        public DateTime? CustomOperationDate { get; set; }
        [Column("isCustomAdded")]
        public bool? IsCustomAdded { get; set; }
        public Operation? Operation { get; set; } = null!; // Required reference navigation to principal
    }
}
