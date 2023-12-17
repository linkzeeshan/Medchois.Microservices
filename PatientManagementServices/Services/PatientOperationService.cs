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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PatientManagementServices.Services
{
    /// <summary>
    /// Patient Operation we will handle all Operation fucntionality in it
    /// </summary>
    public class PatientOperationService : IPatientOperationService
    {
        #region Fields
        private readonly IGenericRepository<Domain.Entities.Operation, long> _OperationRepository;
        private readonly IGenericRepository<PatientOperation, long> _patientOperationRepository;
        private readonly IDapper<ApiResponse<PatientReadDto>> _dapper;
        private readonly IMapper _mapper;
        #endregion

        #region Ctos
        public PatientOperationService(IGenericRepository<Domain.Entities.Operation, long> OperationRepository,
            IGenericRepository<PatientOperation, long> patientOperationRepository, IDapper<ApiResponse<PatientReadDto>> dapper, IMapper mapper)
        {
            _OperationRepository = OperationRepository ?? throw new ArgumentNullException(nameof(OperationRepository));
            _patientOperationRepository = patientOperationRepository ?? throw new ArgumentNullException(nameof(patientOperationRepository));
            _dapper = dapper ?? throw new ArgumentNullException(nameof(Dapper));
            _mapper = mapper;
        }
        #endregion


        #region Functions Operation
        /// <summary>
        /// Get and search patient Operation by id , patient Id, name
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        public async Task<ApiPagingResponse<IEnumerable<OperationReadDto>>> GetAllOperationsAsync(ApiBaseSearchRequest searchRequest)
        {
            ApiPagingResponse<IEnumerable<OperationReadDto>> apiPagingResponse = new ApiPagingResponse<IEnumerable<OperationReadDto>>();
            try
            {
                // return apiPagingResponse;
                var query = _OperationRepository.Queryable().AsNoTracking();
                // Apply filtering based on FirstName and LastName
                if (!string.IsNullOrEmpty(searchRequest.Search))
                {
                    query = query.Where(x => x.OperationName.Contains(searchRequest.Search));
                }

                // Calculate total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var paginatedList = await query
                    .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                    .Take(searchRequest.PageSize)
                    .ToListAsync();

                return new ApiPagingResponse<IEnumerable<OperationReadDto>>
                {
                    PageNumber = searchRequest.PageNumber,
                    PageSize = searchRequest.PageSize,
                    PageCount = (int)Math.Ceiling(totalCount / (double)searchRequest.PageSize),
                    TotalCount = totalCount,
                    Data = _mapper.Map<IEnumerable<OperationReadDto>>(paginatedList),
                    Message = "Success",
                    StatusCode = StatusCodes.Status200OK

                };
            }
            catch(Exception ex)
            {
                apiPagingResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiPagingResponse.Success = false;
                apiPagingResponse.Message = $"{ex.Message} ~ {ex.InnerException}"; 
            }
            return apiPagingResponse;
        }
        /// <summary>
        /// Get Operation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<OperationReadDto>> GetOperationByIdAsync(long id)
        {
            var apiResponse = new ApiResponse<OperationReadDto>();   
            try
            {
                var response = await _OperationRepository.GetByIdAsync(id);
                apiResponse = new ApiResponse<OperationReadDto> { Success = true, Message = "Success", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<OperationReadDto>(response) };
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
        /// Create Operation 
        /// </summary>
        /// <param name="OperationCreateDto"></param>
        /// <returns></returns>
        public async Task<ApiResponse<OperationReadDto>> CreateOperationAsync(OperationCreateDto OperationCreateDto)
        {
            var apiResponse = new ApiResponse<OperationReadDto>();
            try
            {
                //verify create operation is already exist or not
                var isExist = _OperationRepository.Queryable().FirstOrDefault(x => x.OperationName == OperationCreateDto.OperationName);
                if (isExist == null)
                {
                   var response = await _OperationRepository.InsertAsync(_mapper.Map<Domain.Entities.Operation>(OperationCreateDto));
                    return new ApiResponse<OperationReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<OperationReadDto>(response) };
                }
                return new ApiResponse<OperationReadDto> { Success = true, Message= $"{OperationCreateDto.OperationName} is already exist", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<OperationReadDto>(isExist) };
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
        /// update Operation by Operation data
        /// </summary>
        /// <param name="Operation"></param>
        /// <returns></returns>
        public async Task<ApiResponse<OperationReadDto>> UpdateOperationAsync(OperationCreateDto Operation)
        {
            var apiResponse = new ApiResponse<OperationReadDto>();
            try
            {

                var update = await _OperationRepository.UpdateAsync(_mapper.Map<Domain.Entities.Operation>(Operation));
                return new ApiResponse<OperationReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<OperationReadDto>(update) };
            }catch (Exception ex)
            {
                apiResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiResponse;

        }
        /// <summary>
        /// Delete Operation by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeleteOperationAsync(long id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
               var Operation = await _OperationRepository.GetByIdAsync(id);
               if(Operation == null) { return new ApiResponse<bool> { Message = "Not Found", Success = false, StatusCode = StatusCodes.Status404NotFound, Data = false }; }
                Operation.IsActive = false;
                await _OperationRepository.UpdateAsync(Operation);
               apiResponse = new ApiResponse<bool> { Message = $"Operation {Operation.OperationName}  is unactivated successfully", Success = true, StatusCode = StatusCodes.Status200OK, Data = true };
               return apiResponse;

            }catch(Exception ex){
                apiResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiResponse;
        }
        #endregion

        #region Functions Patient Operation
        /// <summary>
        /// Get patient Operation by Id
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        public async Task<ApiPagingResponse<IEnumerable<PatientOperationReadDto>>> GetAllPatientOperationsAsync(ApiBaseSearchRequest searchRequest)
        {
            ApiPagingResponse<IEnumerable<PatientOperationReadDto>> apiPagingResponse = new ApiPagingResponse<IEnumerable<PatientOperationReadDto>>();
            try
            {
                var query =  _patientOperationRepository.Queryable();
                // Apply filtering based on FirstName and LastName
                if(searchRequest.PatientId != 0)
                    query = query.Where(x => x.PatientId == searchRequest.PatientId);

                if (!string.IsNullOrEmpty(searchRequest.Search))
                    query = query.Where(x => x.CustomOperationName.Contains(searchRequest.Search));


                // Calculate total count before pagination
                var totalCount = await query.CountAsync();
                var data  = await query.ToListAsync();
                // Apply pagination
                var paginatedList = await query.AsNoTracking()
                    .Include(x => x.Operation)
                    .Where(x => x.IsActive)
                    .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                    .Take(searchRequest.PageSize)
                    .ToListAsync();
                apiPagingResponse = new ApiPagingResponse<IEnumerable<PatientOperationReadDto>>
                {
                    PageNumber = searchRequest.PageNumber,
                    PageSize = searchRequest.PageSize,
                    PageCount = (int)Math.Ceiling(totalCount / (double)searchRequest.PageSize),
                    TotalCount = totalCount,
                    Data = _mapper.Map<IEnumerable<PatientOperationReadDto>>(paginatedList),
                    Message = "Success",
                    StatusCode = StatusCodes.Status200OK

                };
                return apiPagingResponse;
            }
            catch (Exception ex)
            {
                apiPagingResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiPagingResponse.Success = false;
                apiPagingResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiPagingResponse;
        }
        /// <summary>
        /// Get Patient Allergies by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PatientOperationReadDto>> GetPatientOperationByIdAsync(long id)
        {
            var apiResponse = new ApiResponse<PatientOperationReadDto>();
            try
            {
                var response = await _patientOperationRepository.GetByIdAsync(id);
                apiResponse = new ApiResponse<PatientOperationReadDto> { Success = true, Message = "Success", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientOperationReadDto>(response) };
                return apiResponse;
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
        /// Create patient Operation by patient id
        /// </summary>
        /// <param name="OperationCreateDto"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PatientOperationReadDto>> CreatePatientOperationAsync(PatientOperationCreateDto patientOperationCreateDto)
        {
            var apiResponse = new ApiResponse<PatientOperationReadDto>();
            try
            {
                var isExist = new PatientOperation();
                if (patientOperationCreateDto?.OperationId != null || patientOperationCreateDto?.OperationId != 0)
                    isExist = _patientOperationRepository.Queryable().FirstOrDefault(x => x.OperationId == patientOperationCreateDto.OperationId &&
                    x.PatientId == patientOperationCreateDto.PatientId);
                else
                    isExist = _patientOperationRepository.Queryable().FirstOrDefault(x => x.CustomOperationName == patientOperationCreateDto.CustomOperationName &&
                     x.PatientId == patientOperationCreateDto.PatientId);
                if (isExist == null)
                {
                    var update = await _patientOperationRepository.InsertAsync(_mapper.Map<PatientOperation>(patientOperationCreateDto));
                    return new ApiResponse<PatientOperationReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientOperationReadDto>(update) };
                }
                return new ApiResponse<PatientOperationReadDto> { Success = true, Message = "Operation data is already exist", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientOperationReadDto>(update) };
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
        /// update patient Operation by patient Id and Operation Id
        /// </summary>
        /// <param name="patientOperation"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PatientOperationReadDto>> UpdatePatientOperationAsync(PatientOperationCreateDto patientOperation)
        {
            var apiResponse = new ApiResponse<PatientOperationReadDto>();
            try
            {
                if (_patientOperationRepository.Queryable().FirstOrDefault(x => x.Id == patientOperation.Id) != null)
                {
                   var update =  await _patientOperationRepository.UpdateAsync(_mapper.Map<PatientOperation>(patientOperation));
                    return new ApiResponse<PatientOperationReadDto> { Success = true, Message = "Patient Operation updated", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientOperationReadDto>(update) };
                }
                return new ApiResponse<PatientOperationReadDto> { Success = false, StatusCode = StatusCodes.Status404NotFound, Data = null, Message = "Record Not Found" };
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
        /// Delete patient Operation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeletePatientOperationAsync(long id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
                if (await _patientOperationRepository.Queryable().FirstOrDefaultAsync(x => x.PatientId == id) != null)
                {
                    var patientOperation = await _patientOperationRepository.GetByIdAsync(id);
                    if (patientOperation == null) { return new ApiResponse<bool> { Message = "Alergy ", Success = false, StatusCode = StatusCodes.Status404NotFound, Data = false }; }
                    patientOperation.IsActive = false;
                    await _patientOperationRepository.UpdateAsync(patientOperation);
                    apiResponse = new ApiResponse<bool> { Message = $" Patient Operation is deleted successfully", Success = true, StatusCode = StatusCodes.Status200OK, Data = true };
                    return apiResponse;
                }
                return new ApiResponse<bool> { Success = false, StatusCode = StatusCodes.Status404NotFound, Data = false, Message = "Record Not Found" };

            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiResponse;
        }
        #endregion

       
    }

}
