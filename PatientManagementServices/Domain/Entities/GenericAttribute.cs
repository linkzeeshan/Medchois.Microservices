using PatientManagementServices.Application.Core;
using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Entities
{
    /// <summary>
    /// Represents a generic attribute
    /// </summary>
    [Table("GenericAttribute", Schema = "LocalizationConfiguration")]
    public partial class GenericAttribute: BaseAuditableEntity<long>
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        [Column("EntityId")]
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the key group
        /// </summary>
        [Column("KeyGroup")]
        public string KeyGroup { get; set; }

        /// <summary>
        /// Gets or sets the key
        /// </summary>
        [Column("Key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        [Column("Value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the store identifier
        /// </summary>
        [Column("TenantId")]
        public int TenantId { get; set; }

        /// <summary>
        /// Gets or sets the created or updated date
        /// </summary>
        [Column("CreatedOrUpdatedDateUTC")]
        public DateTime? CreatedOrUpdatedDateUTC { get; set; }
    }
}
