namespace Medchois.UserManagementService.Domain.Contracts
{
    public abstract class BaseAuditableEntity<TId> : IBaseAuditableEntity<TId>
    {
        public TId Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
