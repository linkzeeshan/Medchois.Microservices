using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementServices.Domain.Common
{
    public interface IEntity<TId> : IEntity
    {
        public TId Id { get; set; }
    }

    public interface IEntity
    {
    }
}
