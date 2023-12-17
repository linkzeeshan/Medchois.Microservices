using PatientManagementServices.Application.Common;
using PatientManagementServices.Domain.Dtos;
using PatientManagementServices.Domain.Entities;

namespace PatientManagementServices.Services.Interfaces
{
    public interface IPatientAllergyService
    {
        #region Allergy type Signature
        Task<ApiPagingResponse<IEnumerable<AllergyTypeReadDto>>> GetAllAllergyTypeAsync(ApiBaseSearchRequest searchmodel);
        Task<ApiResponse<AllergyTypeReadDto>> GetAllergyTypeByIdAsync(long id);
        Task<ApiResponse<AllergyTypeReadDto>> CreateAllergyTypeAsync(AllergyTypeCreateDto patientAllergy);
        Task<ApiResponse<AllergyTypeReadDto>> UpdateAllergyTypeAsync(AllergyTypeCreateDto patientAllergy);
        Task<ApiResponse<bool>> DeleteAllergyTypeAsync(long id);
        #endregion

        #region Patient Allergy Signature
        Task<ApiPagingResponse<IEnumerable<PatientAllergyReadDto>>> GetAllPatientergiesAsync(ApiBaseSearchRequest searchmodel);
        Task<ApiResponse<PatientAllergyReadDto>> GetPatientAllergyByIdAsync(long id);
        Task<ApiResponse<PatientAllergyReadDto>> CreatePatientAllergyAsync(PatientAllergyCreateDto patientAllergy);
        Task<ApiResponse<PatientAllergyReadDto>> UpdatePatientAllergyAsync(PatientAllergyCreateDto patientAllergy);
        Task<ApiResponse<bool>> DeletePatientAllergyAsync(long id);
        #endregion

        #region Allergy Signature
        Task<ApiPagingResponse<IEnumerable<AllergyReadDto>>> GetAllAllergiesAsync(ApiBaseSearchRequest searchmodel);
        Task<ApiResponse<AllergyReadDto>> GetAllergyByIdAsync(long id);
        Task<ApiResponse<AllergyReadDto>> CreateAllergyAsync(AllergyCreateDto allergy);
        Task<ApiResponse<AllergyReadDto>> UpdateAllergyAsync(AllergyCreateDto allergy);
        Task<ApiResponse<bool>> DeleteAllergyAsync(long id);
        #endregion
    }
}
