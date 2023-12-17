using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Entities
{
    [Table("Feedback", Schema = "Patient")]
    public class Feedback : BaseAuditableEntity<long>
    {
        [Column("consultationId")]
        public long ConsultationId { get; set; }
        [Column("doctorId")]
        public long DoctorId { get; set; }
        [Column("patientId")]
        public long PatientId { get; set; }
        [Column("rating")]
        public decimal Rating { get; set; }
        [Column("comment")]
        public string Comment { get; set; }
        [Column("feedbackDate")]
        public DateTime FeedbackDate { get; set; }
        public Patient? Patient { get; set; } = null!; // Required reference navigation to principal
    }
}
