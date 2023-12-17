using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementServices.Domain.Common
{
    public abstract class AuditableEntity<TId> : IEntity<TId>
    {
        public TId Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
