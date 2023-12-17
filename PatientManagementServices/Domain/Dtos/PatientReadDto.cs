using PatientManagementServices.Domain.Contracts;
using PatientManagementServices.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientManagementServices.Domain.Dtos
{
    public class PatientReadDto
    {
        public long Id { get; set; }
        public string patientId { get; set; } = string.Empty;
        public long TenantId { get; set; }
        public string? FullName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public bool IsOnline { get; set; }
        public string? Country { get; set; }
        public string? Languages { get; set; } = null;
    }
    public class AllergyTypeReadDto
    {
        public string Name { get; set; } = string.Empty;
        public int? LanguageId { get; set; }
        public bool? IsActive { get; set; }
    }
    public class AllergyReadDto
    {
        public long Id { get; set; }
        public string AllergyName { get; set; } = string.Empty;
        public long AllergyTypeId { get; set; }
        public int? LanguageId { get; set; }
        public bool? IsActive { get; set; }
        public AllergyTypeReadDto? AllergyType { get; set; } = null!; // Required reference navigation to principal
    }
    public class PatientAllergyReadDto
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public long MedicalRecordId { get; set; }
        public long? AllergyId { get; set; }
        public string?CustomAllergyName { get; set; } 
        public string? CustomAllergyType { get; set; }
        public bool? IsCustomAdded { get; set; }
        public bool? IsActive { get; set; }
        public AllergyReadDto? Allergy { get; set; } = null!; // Required reference navigation to principal
    }
    public class BodyMeasurementsReadDTO
    {
        public int Id {  get; set; }
        public int PatientId { get; set; }
        public decimal BodyWeight { get; set; }
        public decimal Height { get; set; }
        public decimal Fats { get; set; }
        public bool IsAllergic { get; set; }
        public bool IsOperation { get; set; }
        public bool IsDisease { get; set; }

    }
    public class PatientOperationReadDto
    {
        public int Id { get; set; }
        public long PatientId { get; set; }
        public long MedicalRecordId { get; set; }
        public int OperationId { get; set; }
        public string CustomOperationName { get; set; } = string.Empty;
        public string CustomOperationDate { get; set; } = string.Empty;
        public bool IsCustomAdded { get; set; }
        public Operation? Operation { get; set; } = null!;
    }
    public class DiseaseReadDto
    {
        public long Id { get; set; }
        public string AllergyName { get; set; } = string.Empty;
        public int LanguageId { get; set; } = 1;
        public string isActive { get; set; }
        public ICollection<PatientDisease> PatientDiseases { get; } = new List<PatientDisease>(); // Collection navigation containing dependents
    }
    public class PatientDiseaseReadDto
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public long medicalRecordId { get; set; }
        public long DiseaseId { get; set; }
        public string CustomDiseaseName { get; set; } 
        public bool IsCustomAdded { get; set; }
        public Disease? Disease { get; set; } = null!; // Required reference navigation to principal

    }
    public class FeedbackReadDto
    {
        public long Id { get; set; }
        public long ConsultationId { get; set; }
        public long DoctorId { get; set; }
        public long PatientId { get; set; }
        public decimal Rating { get; set; }
        public string Comment { get; set; }
        public DateTime FeedbackDate { get; set; }
        public Patient? Patient { get; set; } = null!; // Required reference navigation to principal
    }
    public class OperationReadDto
    {
        public long Id { get; set; }    
        public string OperationName { get; set; } = string.Empty;
        public DateTime OperationDate { get; set; }
        public int LanguageId { get; set; }
        public ICollection<PatientOperation> PatientOperations { get; } = new List<PatientOperation>(); // Collection navigation containing dependents
    }
    public class MedicalRecordReadDto
    {
        public long Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public decimal BodyWeight { get; set; }
        public decimal Height { get; set; }
        public decimal Fats { get; set; }
        public bool IsAllergic { get; set; }
        public bool IsOperation { get; set; }
        public bool IsDisease { get; set; }
        public DateTime DateOfExamination { get; set; }
        public string TreatmentPlan { get; set; }
        public string TestResults { get; set; }
        public DateTime DateOfTest { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
