using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Entities
{
    [Table("MedicalRecord", Schema = "Patient")]
    public class MedicalRecord : BodyMeasurement
    {
        public MedicalRecord() { }
        [Column("doctorId")]
        public int DoctorId { get; set; }
        
        [Column("ddateOfExaminationoctorId")]
        public DateTime DateOfExamination {  get; set; }
        [Column("treatmentPlan")]
        public string TreatmentPlan {  get; set; }
        [Column("testResults")]
        public string TestResults {  get; set; }
        [Column("dateOfTest")]
        public DateTime DateOfTest { get; set; }
        [Column("notes")]
        public string Notes { get; set;}

    }

    public class BodyMeasurement : BaseAuditableEntity<long>
    {
        [Column("patientId")]
        public int PatientId { get; set; }
        [Column("bodyWeight")]
        public decimal BodyWeight { get; set; }
        [Column("height")]
        public decimal Height { get; set; }
        [Column("fats")]
        public decimal Fats { get; set; }
        [Column("isAllergic")]
        public bool IsAllergic { get; set; }
        [Column("isOperation")]
        public bool IsOperation { get; set; }
        [Column("isDisease")]
        public bool IsDisease { get; set; }
    }
}
