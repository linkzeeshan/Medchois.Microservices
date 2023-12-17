namespace Medchois.UserManagementService.Domain.Contracts
{
    public interface IBaseAuditableEntity<TId> : IAuditableEntity, IEntity<TId>
    {
    }

    public interface IAuditableEntity : IEntity
    {
        string CreatedBy { get; set; }

        DateTime CreatedOn { get; set; }

        string UpdatedBy { get; set; }
        //string LastModifiedBy { get; set; }

        DateTime? UpdatedOn { get; set; }
        // DateTime? LastModifiedOn { get; set; }
    }
}
