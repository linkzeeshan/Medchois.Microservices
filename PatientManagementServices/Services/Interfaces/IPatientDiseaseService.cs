using PatientManagementServices.Application.Common;
using PatientManagementServices.Domain.Dtos;
using PatientManagementServices.Domain.Entities;

namespace PatientManagementServices.Services.Interfaces
{
    public interface IPatientDiseaseService
    {

        #region Patient Disease Signature
        Task<ApiPagingResponse<IEnumerable<PatientDiseaseReadDto>>> GetAllPatientDiseasesAsync(ApiBaseSearchRequest searchmodel);
        Task<ApiResponse<PatientDiseaseReadDto>> GetPatientDiseaseByIdAsync(long id);
        Task<ApiResponse<PatientDiseaseReadDto>> CreatePatientDiseaseAsync(PatientDiseaseCreateDto patientDisease);
        Task<ApiResponse<PatientDiseaseReadDto>> UpdatePatientDiseaseAsync(PatientDiseaseCreateDto patientDisease);
        Task<ApiResponse<bool>> DeletePatientDiseaseAsync(long id);
        #endregion

        #region Disease Signature
        Task<ApiPagingResponse<IEnumerable<DiseaseReadDto>>> GetAllDiseasesAsync(ApiBaseSearchRequest searchmodel);
        Task<ApiResponse<DiseaseReadDto>> GetDiseaseByIdAsync(long id);
        Task<ApiResponse<DiseaseReadDto>> CreateDiseaseAsync(DiseaseCreateDto Disease);
        Task<ApiResponse<DiseaseReadDto>> UpdateDiseaseAsync(DiseaseCreateDto Disease);
        Task<ApiResponse<bool>> DeleteDiseaseAsync(long id);
        #endregion
    }
}
