using PatientManagementServices.Domain.Contracts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Dtos
{
    public class PatientCreateDto
    {
        public string patientId { get; set; }
        public string TenantId { get; set; }
        [Required(ErrorMessage ="First Name is requesd")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Second Name is requesd")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Gender is requesd")]
        public string? Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public bool IsOnline { get; set; }
        public string? NationalityId { get; set; }
        public string? Language { get; set; } = null;
    }
    public class AllergyTypeCreateDto
    {
        public long Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public int? LanguageId { get; set; }
    }
    public class AllergyCreateDto
    {
        public long Id { get; set;}
        [Required(ErrorMessage = "Allergy Name is requesd")]
        public string AllergyName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Allergy Type Id is requesd")]
        public long AllergyTypeId { get; set; }
        public int LanguageId { get; set; } = 1;
        public bool IsActive { get; set; }
    }
    public class PatientAllergyCreateDto
    {
        public long Id { get; set; } = 0;
        [Required(ErrorMessage = "Patient Id is requesd")]
        public long PatientId { get; set; }
        public long? medicalRecordId { get; set; }
        public long? allergyId { get; set; } = 0;
        public string? CustomAllergyName { get; set; } //discuss to update in table we will save json in it
        public string? CustomAllergyType { get; set; }
        public bool? IsCustomAdded { get; set; }
    }
    public class DiseaseCreateDto
    {
        public long Id { get; set; } = 0;
        [Required(ErrorMessage = "DiseaseName Name is requesd")]
        public string? DiseaseName { get; set; } = string.Empty;
        public int? LanguageId { get; set; } = 1;
        public bool? isActive { get; set; }
    }
    public class PatientDiseaseCreateDto
    {
        public long Id { get; set; } = 0;
        [Required(ErrorMessage = "Patient Id is requesd")]
        public long PatientId { get; set; }
        public long medicalRecordId { get; set; }
        public long DiseaseId { get; set; }
        public string CustomDiseaseName { get; set; } = string.Empty;
        public bool? isActive { get; set; }
    }
    public class OperationCreateDto
    {
        public long Id { get; set; } = 0;
        public string? OperationName { get; set; } = string.Empty;
        public DateTime? OperationDate { get; set; }
        public int? LanguageId { get; set; }
    }
    public class PatientOperationCreateDto
    {
        public long Id { get; set; } = 0;
        [Required(ErrorMessage = "Patient Id is requesd")]
        public long? PatientId { get; set; }
        public long? MedicalRecordId { get; set; }
        public long? OperationId { get; set; }
        public string? CustomOperationName { get; set; } = string.Empty;
        public DateTime? CustomOperationDate { get; set; }
        public bool? IsCustomAdded { get; set; }
        //public Operation? Operation { get; set; } = null!; // Required reference navigation to principal
    }
    public class FeedbackCreateDto
    {
        public long Id { get; set; } = 0;
        public long? ConsultationId { get; set; }
        public long DoctorId { get; set; }
        public long PatientId { get; set; }
        public decimal? Rating { get; set; }
        public string? Comment { get; set; } = string.Empty;
        public DateTime FeedbackDate { get; set; }
    }
    public class MedicalRecordCreateDto
    {
        public long Id { get; set; } = 0;
        public long? DoctorId { get; set; }
        [Required(ErrorMessage = "Patient Id is requesd")]
        public long? PatientId { get; set; }
        public decimal? BodyWeight { get; set; }
        public decimal? Height { get; set; }
        public decimal? Fats { get; set; }
        public bool? IsAllergic { get; set; }
        public bool IsOperation { get; set; }
        public bool? IsDisease { get; set; }
        public DateTime? DateOfExamination { get; set; }
        public string? TreatmentPlan { get; set; } = string.Empty;
        public string? TestResults { get; set; }
        public DateTime? DateOfTest { get; set; }
        public string? Notes { get; set; }
        public bool? IsActive { get; set; }
    }
    public class PrescriptionCreateDto
    {
        public long Id { get; set; } = 0;
        [Required(ErrorMessage = "Doctor Id is requesd")]
        public long? DoctorId { get; set; }
        [Required(ErrorMessage = "Patient Id is requesd")]
        public long? PatientId { get; set; }
        public string? MedicationName { get; set; }
        public string? Dosage { get; set; }
        public string? Instructions { get; set; } //discuss to update in table we will save json in it
        public DateTime? DatePrescribed { get; set; }
    }
}
