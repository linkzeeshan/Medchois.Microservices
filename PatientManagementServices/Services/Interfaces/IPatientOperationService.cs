using PatientManagementServices.Application.Common;
using PatientManagementServices.Domain.Dtos;
using PatientManagementServices.Domain.Entities;

namespace PatientManagementServices.Services.Interfaces
{
    public interface IPatientOperationService
    {

        #region Patient Operation Signature
        Task<ApiPagingResponse<IEnumerable<PatientOperationReadDto>>> GetAllPatientOperationsAsync(ApiBaseSearchRequest searchmodel);
        Task<ApiResponse<PatientOperationReadDto>> GetPatientOperationByIdAsync(long id);
        Task<ApiResponse<PatientOperationReadDto>> CreatePatientOperationAsync(PatientOperationCreateDto patientOperation);
        Task<ApiResponse<PatientOperationReadDto>> UpdatePatientOperationAsync(PatientOperationCreateDto patientOperation);
        Task<ApiResponse<bool>> DeletePatientOperationAsync(long id);
        #endregion

        #region Operation Signature
        Task<ApiPagingResponse<IEnumerable<OperationReadDto>>> GetAllOperationsAsync(ApiBaseSearchRequest searchmodel);
        Task<ApiResponse<OperationReadDto>> GetOperationByIdAsync(long id);
        Task<ApiResponse<OperationReadDto>> CreateOperationAsync(OperationCreateDto Operation);
        Task<ApiResponse<OperationReadDto>> UpdateOperationAsync(OperationCreateDto Operation);
        Task<ApiResponse<bool>> DeleteOperationAsync(long id);
        #endregion
    }
}
