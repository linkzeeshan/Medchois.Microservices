using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Contracts
{
    public abstract class BaseAuditableEntity<TId> : IBaseAuditableEntity<TId>
    {
        public TId Id { get; set; }
        [Column("createdBy")]
        public string? CreatedBy { get; set; }
        [Column("createdDate")]
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        [Column("updatedBy")]
        public string? UpdatedBy { get; set; }
        [Column("updatedDate")]
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        [Column("isActive")]
        public bool IsActive { get; set; } = false;
    }
}
