using AutoMapper;
using PatientManagementServices.Domain.Dtos;
using PatientManagementServices.Domain.Entities;

namespace PatientManagementServices.Domain.Mappers
{
    public class PatientMappings : Profile
    {
        public PatientMappings()
        {
            CreateMap<Patient, PatientCreateDto>().ReverseMap();
            CreateMap<PatientReadDto, Patient>().ReverseMap();
            CreateMap<PatientReadDto, PatientCreateDto>().ReverseMap();

            CreateMap<Allergy, AllergyCreateDto>().ReverseMap();
            CreateMap<Disease, DiseaseCreateDto>().ReverseMap();
            CreateMap<Feedback, FeedbackCreateDto>().ReverseMap();
            CreateMap<MedicalRecord, MedicalRecordCreateDto>().ReverseMap();
            CreateMap<Operation, OperationCreateDto>().ReverseMap();
            CreateMap<PatientAllergy, PatientAllergyCreateDto>().ReverseMap();
            CreateMap<PatientDisease, PatientDiseaseCreateDto>().ReverseMap();
            CreateMap<PatientOperation, PatientOperationCreateDto>().ReverseMap();
            CreateMap<Prescription, PrescriptionCreateDto>().ReverseMap();
            CreateMap<AllergyType, AllergyTypeCreateDto>().ReverseMap();

            CreateMap<Allergy, AllergyReadDto>().ReverseMap();
            CreateMap<PatientAllergy, PatientAllergyReadDto>().ReverseMap();
            CreateMap<AllergyType, AllergyTypeReadDto>().ReverseMap();
            CreateMap<Disease, DiseaseReadDto>().ReverseMap();
            CreateMap<Feedback, FeedbackReadDto>().ReverseMap();
            CreateMap<MedicalRecord, MedicalRecordReadDto>().ReverseMap();
            CreateMap<Operation, OperationReadDto>().ReverseMap();
            CreateMap<PatientOperation, PatientOperationReadDto>().ReverseMap();

        }
    }
}
