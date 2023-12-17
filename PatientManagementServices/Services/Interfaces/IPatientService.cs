using PatientManagementServices.Application.Common;
using PatientManagementServices.Domain.Dtos;
using PatientManagementServices.Domain.Entities;

namespace PatientManagementServices.Services.Interfaces
{
    public interface IPatientService
    {
        Task<ApiPagingResponse<IEnumerable<PatientReadDto>>> GetAllPatientsAsync(ApiBaseSearchRequest searchmodel);
        Task<ApiResponse<PatientReadDto>> GetPatientByIdAsync(int id);
        Task<ApiResponse<PatientReadDto>> CreatePatientAsync(PatientCreateDto patient);
        Task<ApiResponse<PatientReadDto>> UpdatePatientAsync(PatientCreateDto patient);
        Task<ApiResponse<bool>> DeletePatientAsync(int id);
    }
}
