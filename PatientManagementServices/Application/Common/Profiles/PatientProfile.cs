using AutoMapper;
using PatientManagementServices.Domain.Dtos;
using PatientManagementServices.Domain.Entities;

namespace PatientManagementServices.Application.Common.Profiles
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            // Source -> Target
            CreateMap<Patient, PatientCreateDto>().ReverseMap();
        }
       
        
    }
}
