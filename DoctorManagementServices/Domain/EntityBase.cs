using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementServices.Domain
{
    public class EntityBase
    {

        [Column("ID")]
        public virtual string Id { get; set; }

        [Column("CREATED_DATE")]
        public virtual DateTime CreatedDate { get; set; }
        [Column("CREATED_BY")]
        public virtual string CreatedBy { get; set; }

        [Column("UPDATED_DATE")]
        public virtual DateTime? UpdatedDate { get; set; }
        [Column("UPDATED_BY")]
        public virtual string UpdatedBy { get; set; }
    }
}
