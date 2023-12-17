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
    /// Patient Disease we will handle all Disease fucntionality in it
    /// </summary>
    public class PatientDiseaseService : IPatientDiseaseService
    {
        #region Fields
        private readonly IGenericRepository<Disease, long> _diseaseRepository;
        private readonly IGenericRepository<PatientDisease, long> _patientDiseaseRepository;
        private readonly IDapper<ApiResponse<PatientDiseaseReadDto>> _dapper;
        private readonly IMapper _mapper;
        #endregion

        #region Ctos
        public PatientDiseaseService(IGenericRepository<Disease, long> diseaseRepository, 
            IGenericRepository<PatientDisease, long> patientDiseaseRepository, 
            IDapper<ApiResponse<PatientDiseaseReadDto>> dapper, IMapper mapper)
        {
            _diseaseRepository = diseaseRepository ?? throw new ArgumentNullException(nameof(diseaseRepository));
            _patientDiseaseRepository = patientDiseaseRepository ?? throw new ArgumentNullException(nameof(patientDiseaseRepository));
            _dapper = dapper ?? throw new ArgumentNullException(nameof(Dapper));
            _mapper = mapper;
        }
        #endregion


        #region Functions Disease
        /// <summary>
        /// Get and search patient Disease by id , patient Id, name
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        public async Task<ApiPagingResponse<IEnumerable<DiseaseReadDto>>> GetAllDiseasesAsync(ApiBaseSearchRequest searchRequest)
        {
            ApiPagingResponse<IEnumerable<DiseaseReadDto>> apiPagingResponse = new ApiPagingResponse<IEnumerable<DiseaseReadDto>>();
            try
            {
                // return apiPagingResponse;
                var query = _diseaseRepository.Queryable().AsNoTracking();
                // Apply filtering based on FirstName and LastName
                if (!string.IsNullOrEmpty(searchRequest.Search))
                {
                    query = query.Where(x => x.DiseaseName.Contains(searchRequest.Search));
                }

                // Calculate total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var paginatedList = await query
                    .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                    .Take(searchRequest.PageSize)
                    .ToListAsync();

                return new ApiPagingResponse<IEnumerable<DiseaseReadDto>>
                {
                    PageNumber = searchRequest.PageNumber,
                    PageSize = searchRequest.PageSize,
                    PageCount = (int)Math.Ceiling(totalCount / (double)searchRequest.PageSize),
                    TotalCount = totalCount,
                    Data = _mapper.Map<IEnumerable<DiseaseReadDto>>(paginatedList),
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
        /// Get Disease by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<DiseaseReadDto>> GetDiseaseByIdAsync(long id)
        {
            var apiResponse = new ApiResponse<DiseaseReadDto>();   
            try
            {
                var response = await _diseaseRepository.GetByIdAsync(id);
                apiResponse = new ApiResponse<DiseaseReadDto> { Success = true, Message = "Success", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<DiseaseReadDto>(response) };
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
        /// Create Disease 
        /// </summary>
        /// <param name="DiseaseCreateDto"></param>
        /// <returns></returns>
        public async Task<ApiResponse<DiseaseReadDto>> CreateDiseaseAsync(DiseaseCreateDto diseaseCreateDto)
        {
            var apiResponse = new ApiResponse<DiseaseReadDto>();
            try
            {
                if (_diseaseRepository.Queryable().FirstOrDefault(x => x.DiseaseName == diseaseCreateDto.DiseaseName) == null)
                {
                    await _diseaseRepository.InsertAsync(_mapper.Map<Domain.Entities.Disease>(diseaseCreateDto));
                    var data = _diseaseRepository.Queryable().AsNoTracking().FirstOrDefault(x => x.DiseaseName.ToLower() == diseaseCreateDto.DiseaseName.ToLower());
                    return new ApiResponse<DiseaseReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<DiseaseReadDto>(data) };
                }
                return new ApiResponse<DiseaseReadDto> { Success = true, Message = $"{diseaseCreateDto.DiseaseName} is already exist", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<DiseaseReadDto>(data) };
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
        /// update Disease by Disease data
        /// </summary>
        /// <param name="Disease"></param>
        /// <returns></returns>
        public async Task<ApiResponse<DiseaseReadDto>> UpdateDiseaseAsync(DiseaseCreateDto Disease)
        {
            var apiResponse = new ApiResponse<DiseaseReadDto>();
            try
            {
                var update = await _diseaseRepository.UpdateAsync(_mapper.Map<Domain.Entities.Disease>(Disease));
                return new ApiResponse<DiseaseReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<DiseaseReadDto>(update) };
            }catch (Exception ex)
            {
                apiResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiResponse;

        }
        /// <summary>
        /// Delete Disease by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeleteDiseaseAsync(long id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
               var Disease = await _diseaseRepository.GetByIdAsync(id);
               if(Disease == null) { return new ApiResponse<bool> { Message = "Not Found", Success = false, StatusCode = StatusCodes.Status404NotFound, Data = false }; }
                Disease.IsActive = false;
                await _diseaseRepository.UpdateAsync(Disease);
               apiResponse = new ApiResponse<bool> { Message = $"Disease {Disease.DiseaseName}  is unactivated successfully", Success = true, StatusCode = StatusCodes.Status200OK, Data = true };
               return apiResponse;

            }catch(Exception ex){
                apiResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiResponse;
        }
        #endregion

        #region Functions Patient Disease
        /// <summary>
        /// Get patient Disease by Id
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        public async Task<ApiPagingResponse<IEnumerable<PatientDiseaseReadDto>>> GetAllPatientDiseasesAsync(ApiBaseSearchRequest searchRequest)
        {
            ApiPagingResponse<IEnumerable<PatientDiseaseReadDto>> apiPagingResponse = new ApiPagingResponse<IEnumerable<PatientDiseaseReadDto>>();
            try
            {
                var query =  _patientDiseaseRepository.Queryable();
                // Apply filtering based on FirstName and LastName
                if(searchRequest.PatientId != 0)
                    query = query.Where(x => x.PatientId == searchRequest.PatientId);

                if (!string.IsNullOrEmpty(searchRequest.Search))
                    query = query.Where(x => x.CustomDiseaseName.Contains(searchRequest.Search));


                // Calculate total count before pagination
                var totalCount = await query.CountAsync();
                var data  = await query.ToListAsync();
                // Apply pagination
                var paginatedList = await query.AsNoTracking()
                    .Include(x => x.Disease)
                    .Where(x => x.IsActive)
                    .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                    .Take(searchRequest.PageSize)
                    .ToListAsync();
                apiPagingResponse = new ApiPagingResponse<IEnumerable<PatientDiseaseReadDto>>
                {
                    PageNumber = searchRequest.PageNumber,
                    PageSize = searchRequest.PageSize,
                    PageCount = (int)Math.Ceiling(totalCount / (double)searchRequest.PageSize),
                    TotalCount = totalCount,
                    Data = _mapper.Map<IEnumerable<PatientDiseaseReadDto>>(paginatedList),
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
        public async Task<ApiResponse<PatientDiseaseReadDto>> GetPatientDiseaseByIdAsync(long id)
        {
            var apiResponse = new ApiResponse<PatientDiseaseReadDto>();
            try
            {
                var response = await _patientDiseaseRepository.GetByIdAsync(id);
                apiResponse = new ApiResponse<PatientDiseaseReadDto> { Success = true, Message = "Success", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientDiseaseReadDto>(response) };
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
        /// Create patient Disease by patient id
        /// </summary>
        /// <param name="DiseaseCreateDto"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PatientDiseaseReadDto>> CreatePatientDiseaseAsync(PatientDiseaseCreateDto diseaseCreateDto)
        {
            var apiResponse = new ApiResponse<PatientDiseaseReadDto>();
            try
            {
                var isExist = new PatientDisease();
                if (diseaseCreateDto?.DiseaseId != 0 && diseaseCreateDto?.DiseaseId != null)
                    isExist = _patientDiseaseRepository.Queryable().FirstOrDefault(x => x.DiseaseId == diseaseCreateDto.DiseaseId && x.PatientId == diseaseCreateDto.PatientId);
                if (isExist == null)
                {
                    var resposne = await _patientDiseaseRepository.InsertAsync(_mapper.Map<PatientDisease>(diseaseCreateDto));
                    return new ApiResponse<PatientDiseaseReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientDiseaseReadDto>(resposne) };
                }
                return new ApiResponse<PatientDiseaseReadDto> { Success = true, Message = $"Enter disease is already exist", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientDiseaseReadDto>(isExist) };
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
        /// update patient Disease by patient Id and Disease Id
        /// </summary>
        /// <param name="patientDisease"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PatientDiseaseReadDto>> UpdatePatientDiseaseAsync(PatientDiseaseCreateDto patientDisease)
        {
            var apiResponse = new ApiResponse<PatientDiseaseReadDto>();
            try
            {
                if (_patientDiseaseRepository.Queryable().FirstOrDefault(x => x.Id == patientDisease.Id) != null)
                {
                   var update =  await _patientDiseaseRepository.UpdateAsync(_mapper.Map<PatientDisease>(patientDisease));
                    return new ApiResponse<PatientDiseaseReadDto> { Success = true, Message = "Patient Disease updated", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientDiseaseReadDto>(update) };
                }
                return new ApiResponse<PatientDiseaseReadDto> { Success = false, StatusCode = StatusCodes.Status404NotFound, Data = null, Message = "Record Not Found" };
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
        /// Delete patient Disease by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeletePatientDiseaseAsync(long id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
                if (await _patientDiseaseRepository.Queryable().FirstOrDefaultAsync(x => x.PatientId == id) != null)
                {
                    var patientDisease = await _patientDiseaseRepository.GetByIdAsync(id);
                    if (patientDisease == null) { return new ApiResponse<bool> { Message = "Alergy ", Success = false, StatusCode = StatusCodes.Status404NotFound, Data = false }; }
                    patientDisease.IsActive = false;
                    await _patientDiseaseRepository.UpdateAsync(patientDisease);
                    apiResponse = new ApiResponse<bool> { Message = $" Patient Disease is deleted successfully", Success = true, StatusCode = StatusCodes.Status200OK, Data = true };
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
