using AutoMapper;
using Azure;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PatientManagementServices.Application.Common;
using PatientManagementServices.Domain.Dtos;
using PatientManagementServices.Domain.Entities;
using PatientManagementServices.Domain.Interfaces;
using PatientManagementServices.Domain.Interfaces.IRepository;
using PatientManagementServices.Domain.Mappers;
using PatientManagementServices.Services.Interfaces;
using System.Collections.Generic;

namespace PatientManagementServices.Services
{
    /// <summary>
    /// Patient service implementation service which have handling all requirement regarding to patient activity
    /// </summary>
    public class PatientService : IPatientService
    {
        #region Fields
        private readonly IPatientRepository _patientRepository;
        private readonly IDapper<ApiResponse<PatientReadDto>> _dapper;
        private readonly IMapper _mapper;
        #endregion

        #region Ctos
        public PatientService(IPatientRepository patientRepository, IDapper<ApiResponse<PatientReadDto>> dapper, IMapper mapper)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _dapper = dapper ?? throw new ArgumentNullException(nameof(Dapper));
            _mapper = mapper;
        }
        #endregion
        #region Functions
        /// <summary>
        /// Get All Patients base on user serach
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        public async Task<ApiPagingResponse<IEnumerable<PatientReadDto>>> GetAllPatientsAsync(ApiBaseSearchRequest searchmodel)
        {
            ApiResponse<IEnumerable<PatientReadDto>> apiResponse = new ApiResponse<IEnumerable<PatientReadDto>>();
            ApiPagingResponse<IEnumerable<PatientReadDto>> apiPagingResponse = new ApiPagingResponse<IEnumerable<PatientReadDto>>();
            try
            {
                //Assign dynamci paramerts for dapper
                DynamicParameters param = DapperParametersMapping.DynamicParameters(searchmodel);
                var dapper_response = await _dapper.GetDataListAsync<ApiPagingResponse<IEnumerable<PatientReadDto>>>("usp_SearchPatient", param, System.Data.CommandType.StoredProcedure);
                var response = _mapper.Map<IEnumerable<PatientReadDto>>(await _patientRepository.Queryable().ToListAsync());
                apiResponse = new ApiResponse<IEnumerable<PatientReadDto>> { Success = true, Message = "Success", StatusCode = StatusCodes.Status200OK, Data = response };
                return apiPagingResponse;
            }catch(Exception ex)
            {
                apiPagingResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiPagingResponse.Success = false;
                apiPagingResponse.Message = $"{ex.Message} ~ {ex.InnerException}"; 
            }
            return apiPagingResponse;
        }
        /// <summary>
        /// Get patient information by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PatientReadDto>> GetPatientByIdAsync(int id)
        {
            var apiResponse = new ApiResponse<PatientReadDto>();   
            try
            {
                var response = await _patientRepository.GetByIdAsync(id);
                apiResponse = new ApiResponse<PatientReadDto> { Success = true, Message = "Success", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientReadDto>(response) };
                return apiResponse;
            }catch(Exception ex)
            {
                apiResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiResponse;
        }
        /// <summary>
        /// Create patient information
        /// </summary>
        /// <param name="patientCreateDto"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PatientReadDto>> CreatePatientAsync(PatientCreateDto patientCreateDto)
        {
            var apiResponse = new ApiResponse<PatientReadDto>();
            try
            {
                await _patientRepository.InsertAsync(_mapper.Map<Patient>(patientCreateDto));
                var data = _patientRepository.Queryable().AsNoTracking().FirstOrDefault(x => x.patientId == patientCreateDto.patientId.ToString());
                return new ApiResponse<PatientReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientReadDto>(data) };
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiResponse;

        }
        /// <summary>
        /// update current patient
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PatientReadDto>> UpdatePatientAsync(PatientCreateDto patient)
        {
            var apiResponse = new ApiResponse<PatientReadDto>();
            try
            {
                await _patientRepository.UpdateAsync(_mapper.Map<Patient>(patient));
                return new ApiResponse<PatientReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientReadDto>(patient) };
            }catch (Exception ex)
            {
                apiResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiResponse;

        }
        /// <summary>
        /// Delete current patient
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeletePatientAsync(int id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
               var patient = await _patientRepository.GetByIdAsync(id);
               if(patient == null) { return new ApiResponse<bool> { Message = "Patient ", Success = false, StatusCode = StatusCodes.Status404NotFound, Data = false }; }
               await _patientRepository.DeleteAsync(id);
               apiResponse = new ApiResponse<bool> { Message = $"Patient {patient.FirstName} {patient.LastName} is deleted successfully", Success = true, StatusCode = StatusCodes.Status200OK, Data = true };
               return apiResponse;

            }catch(Exception ex){
                apiResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiResponse;
        }
        #endregion
    }

}
