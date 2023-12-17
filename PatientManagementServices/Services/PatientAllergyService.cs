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
    /// Patient Allergy we will handle all allergy fucntionality in it
    /// </summary>
    public class PatientAllergyService : IPatientAllergyService
    {
        #region Fields
        private readonly IGenericRepository<AllergyType, long> _allergyTypeRepository;
        private readonly IGenericRepository<Allergy, long> _allergyRepository;
        private readonly IGenericRepository<PatientAllergy, long> _patientAllergyRepository;
        private readonly IDapper<ApiResponse<PatientReadDto>> _dapper;
        private readonly IMapper _mapper;
        #endregion

        #region Ctos
        public PatientAllergyService(IGenericRepository<AllergyType, long> allergyTypeRepository,
            IGenericRepository<Allergy, long> allergyRepository,
            IGenericRepository<PatientAllergy, long> patientAllergyRepository,
            IDapper<ApiResponse<PatientReadDto>> dapper, IMapper mapper)
        {
            _allergyTypeRepository = allergyTypeRepository ?? throw new ArgumentNullException(nameof(allergyTypeRepository));
            _allergyRepository = allergyRepository ?? throw new ArgumentNullException(nameof(allergyRepository));
            _patientAllergyRepository = patientAllergyRepository ?? throw new ArgumentNullException(nameof(patientAllergyRepository));
            _dapper = dapper ?? throw new ArgumentNullException(nameof(Dapper));
            _mapper = mapper;
        }
        #endregion

        #region Functions Allergy Type
        /// <summary>
        /// This is allergy type
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        public async Task<ApiPagingResponse<IEnumerable<AllergyTypeReadDto>>> GetAllAllergyTypeAsync(ApiBaseSearchRequest searchRequest)
        {
            ApiPagingResponse<IEnumerable<AllergyTypeReadDto>> apiPagingResponse = new ApiPagingResponse<IEnumerable<AllergyTypeReadDto>>();
            try
            {
                // return apiPagingResponse;
                var query = _allergyTypeRepository.Queryable().AsNoTracking();
                // Apply filtering based on FirstName and LastName
                if (!string.IsNullOrEmpty(searchRequest.FirstName))
                {
                    query = query.Where(x => x.Name.Contains(searchRequest.Search));
                }

                // Calculate total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var paginatedList = await query
                    .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                    .Take(searchRequest.PageSize)
                    .ToListAsync();

                return new ApiPagingResponse<IEnumerable<AllergyTypeReadDto>>
                {
                    PageNumber = searchRequest.PageNumber,
                    PageSize = searchRequest.PageSize,
                    PageCount = (int)Math.Ceiling(totalCount / (double)searchRequest.PageSize),
                    TotalCount = totalCount,
                    Data = _mapper.Map<IEnumerable<AllergyTypeReadDto>>(paginatedList),
                    Message = "Success",
                    StatusCode = StatusCodes.Status200OK

                };
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
        /// Get Allergy by type id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<AllergyTypeReadDto>> GetAllergyTypeByIdAsync(long id)
        {
            var apiResponse = new ApiResponse<AllergyTypeReadDto>();
            try
            {
                var response = await _allergyTypeRepository.GetByIdAsync(id);
                apiResponse = new ApiResponse<AllergyTypeReadDto> { Success = true, Message = "Success", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<AllergyTypeReadDto>(response) };
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
        /// Add Allergy type 
        /// </summary>
        /// <param name="allergyType"></param>
        /// <returns></returns>
        public async Task<ApiResponse<AllergyTypeReadDto>> CreateAllergyTypeAsync(AllergyTypeCreateDto allergyType)
        {
            var apiResponse = new ApiResponse<AllergyTypeReadDto>();
            try
            {
                var isExist = _allergyTypeRepository.Queryable().FirstOrDefaultAsync(x => x.Name.ToLower() == allergyType.Name.ToLower());
                if (isExist != null)
                {
                    var response = await _allergyTypeRepository.InsertAsync(_mapper.Map<AllergyType>(allergyType));
                    return new ApiResponse<AllergyTypeReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<AllergyTypeReadDto>(response) };
                }
                return new ApiResponse<AllergyTypeReadDto> { Success = true, Message = $"{allergyType.Name} is already exist", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<AllergyTypeReadDto>(isExist) };
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
        /// update allergy type
        /// </summary>
        /// <param name="allergyTypeCreateDto"></param>
        /// <returns></returns>
        public async Task<ApiResponse<AllergyTypeReadDto>> UpdateAllergyTypeAsync(AllergyTypeCreateDto allergyTypeCreateDto)
        {
            var apiResponse = new ApiResponse<AllergyTypeReadDto>();
            try
            {
                var dataa = _mapper.Map<AllergyType>(allergyTypeCreateDto);
                var update = await _allergyTypeRepository.UpdateAsync(_mapper.Map<AllergyType>(allergyTypeCreateDto));
                return new ApiResponse<AllergyTypeReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<AllergyTypeReadDto>(update) };
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
        /// Delete Allergy type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeleteAllergyTypeAsync(long id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
                var allergyType = await _allergyTypeRepository.GetByIdAsync(id);
                if (allergyType == null) { return new ApiResponse<bool> { Message = "Alergy Type", Success = false, StatusCode = StatusCodes.Status404NotFound, Data = false }; }
                allergyType.IsActive = false;
                var update = await _allergyTypeRepository.UpdateAsync(allergyType);
                apiResponse = new ApiResponse<bool> { Message = $"Allergy {allergyType.Name} is un active successfully", Success = true, StatusCode = StatusCodes.Status200OK, Data = true };
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
        #endregion

        #region Functions Allergy
        /// <summary>
        /// Get and search patient allergy by id , patient Id, name
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        public async Task<ApiPagingResponse<IEnumerable<AllergyReadDto>>> GetAllAllergiesAsync(ApiBaseSearchRequest searchRequest)
        {
            ApiPagingResponse<IEnumerable<AllergyReadDto>> apiPagingResponse = new ApiPagingResponse<IEnumerable<AllergyReadDto>>();
            try
            {
                // return apiPagingResponse;
                var query = _allergyRepository.Queryable().AsNoTracking();
                // Apply filtering based on FirstName and LastName
                if (!string.IsNullOrEmpty(searchRequest.FirstName))
                {
                    query = query.Where(x => x.AllergyName.Contains(searchRequest.Search));
                }

                if (!string.IsNullOrEmpty(searchRequest.Search))
                {
                    query = query.Where(x => x.AllergyType.Name.Contains(searchRequest.Search));
                }

                // Calculate total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var paginatedList = await query
                    .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                    .Take(searchRequest.PageSize)
                    .ToListAsync();

                return new ApiPagingResponse<IEnumerable<AllergyReadDto>>
                {
                    PageNumber = searchRequest.PageNumber,
                    PageSize = searchRequest.PageSize,
                    PageCount = (int)Math.Ceiling(totalCount / (double)searchRequest.PageSize),
                    TotalCount = totalCount,
                    Data = _mapper.Map<IEnumerable<AllergyReadDto>>(paginatedList),
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
        /// Get Allergy by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<AllergyReadDto>> GetAllergyByIdAsync(long id)
        {
            var apiResponse = new ApiResponse<AllergyReadDto>();   
            try
            {
                var response = await _allergyRepository.GetByIdAsync(id);
                apiResponse = new ApiResponse<AllergyReadDto> { Success = true, Message = "Success", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<AllergyReadDto>(response) };
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
        /// Create Allergy 
        /// </summary>
        /// <param name="allergyCreateDto"></param>
        /// <returns></returns>
        public async Task<ApiResponse<AllergyReadDto>> CreateAllergyAsync(AllergyCreateDto allergyCreateDto)
        {
            var apiResponse = new ApiResponse<AllergyReadDto>();
            try
            {
                var isExist = _allergyRepository.Queryable().AsNoTracking().FirstOrDefaultAsync(x => x.AllergyName.ToLower() == allergyCreateDto.AllergyName.ToLower() && x.AllergyTypeId == allergyCreateDto.AllergyTypeId);
                if (isExist != null)
                {
                    var response = await _allergyRepository.InsertAsync(_mapper.Map<Allergy>(allergyCreateDto));
                    return new ApiResponse<AllergyReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<AllergyReadDto>(response) };
                }
                return new ApiResponse<AllergyReadDto> { Success = true, Message = $"{allergyCreateDto.AllergyName} is exist in same allergy type", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<AllergyReadDto>(isExist) };
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
        /// update allergy by allergy data
        /// </summary>
        /// <param name="allergy"></param>
        /// <returns></returns>
        public async Task<ApiResponse<AllergyReadDto>> UpdateAllergyAsync(AllergyCreateDto allergy)
        {
            var apiResponse = new ApiResponse<AllergyReadDto>();
            try
            {
                var update = await _allergyRepository.UpdateAsync(_mapper.Map<Allergy>(allergy));
                return new ApiResponse<AllergyReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<AllergyReadDto>(update) };
            }catch (Exception ex)
            {
                apiResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiResponse;

        }
        /// <summary>
        /// Delete Allergy by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeleteAllergyAsync(long id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
               var allergy = await _allergyRepository.GetByIdAsync(id);
               if(allergy == null) { return new ApiResponse<bool> { Message = "Alergy ", Success = false, StatusCode = StatusCodes.Status404NotFound, Data = false }; }
                allergy.IsActive = false;
                await _allergyRepository.UpdateAsync(allergy);
               apiResponse = new ApiResponse<bool> { Message = $"Allergy {allergy.AllergyName} {allergy.AllergyType} is unactivated successfully", Success = true, StatusCode = StatusCodes.Status200OK, Data = true };
               return apiResponse;

            }catch(Exception ex){
                apiResponse.StatusCode = StatusCodes.Status400BadRequest;
                apiResponse.Success = false;
                apiResponse.Message = $"{ex.Message} ~ {ex.InnerException}";
            }
            return apiResponse;
        }
        #endregion

        #region Functions Patient Allergy
        /// <summary>
        /// Get patient Allergy by Id
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        public async Task<ApiPagingResponse<IEnumerable<PatientAllergyReadDto>>> GetAllPatientergiesAsync(ApiBaseSearchRequest searchRequest)
        {
            ApiPagingResponse<IEnumerable<PatientAllergyReadDto>> apiPagingResponse = new ApiPagingResponse<IEnumerable<PatientAllergyReadDto>>();
            try
            {
                var query =  _patientAllergyRepository.Queryable();
                // Apply filtering based on FirstName and LastName
                if(searchRequest.PatientId != 0)
                    query = query.Where(x => x.PatientId == searchRequest.PatientId);

                if (!string.IsNullOrEmpty(searchRequest.Search))
                    query = query.Where(x => x.CustomAllergyName.Contains(searchRequest.Search));

                if (!string.IsNullOrEmpty(searchRequest.LastName))
                    query = query.Where(x => x.CustomAllergyType.Contains(searchRequest.Search));

                // Calculate total count before pagination
                var totalCount = await query.CountAsync();
                var data  = await query.ToListAsync();
                // Apply pagination
                var paginatedList = await query.AsNoTracking()
                    .Include(x => x.Allergy)
                    .Where(x => x.IsActive)
                    .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                    .Take(searchRequest.PageSize)
                    .ToListAsync();
                apiPagingResponse = new ApiPagingResponse<IEnumerable<PatientAllergyReadDto>>
                {
                    PageNumber = searchRequest.PageNumber,
                    PageSize = searchRequest.PageSize,
                    PageCount = (int)Math.Ceiling(totalCount / (double)searchRequest.PageSize),
                    TotalCount = totalCount,
                    Data = _mapper.Map<IEnumerable<PatientAllergyReadDto>>(paginatedList),
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
        public async Task<ApiResponse<PatientAllergyReadDto>> GetPatientAllergyByIdAsync(long id)
        {
            var apiResponse = new ApiResponse<PatientAllergyReadDto>();
            try
            {
                var response = await _patientAllergyRepository.GetByIdAsync(id);
                apiResponse = new ApiResponse<PatientAllergyReadDto> { Success = true, Message = "Success", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientAllergyReadDto>(response) };
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
        /// Create patient Allergy by patient id
        /// </summary>
        /// <param name="allergyCreateDto"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PatientAllergyReadDto>> CreatePatientAllergyAsync(PatientAllergyCreateDto allergyCreateDto)
        {
            var apiResponse = new ApiResponse<PatientAllergyReadDto>();
            try
            {
                var isExist = new PatientAllergy();
                if(allergyCreateDto != null && (allergyCreateDto?.allergyId != null || allergyCreateDto?.allergyId != 0))
                 isExist = await _patientAllergyRepository.Queryable().AsNoTracking().FirstOrDefaultAsync(x => x.AllergyId == allergyCreateDto.allergyId && x.PatientId == allergyCreateDto.PatientId);
                else
                    isExist = await _patientAllergyRepository.Queryable().AsNoTracking().FirstOrDefaultAsync(x => x.CustomAllergyName.ToLower() == allergyCreateDto.CustomAllergyName.ToLower() &&
                    x.CustomAllergyType.ToLower() == allergyCreateDto.CustomAllergyType.ToLower());

                if (isExist == null)
                {
                    await _patientAllergyRepository.InsertAsync(_mapper.Map<PatientAllergy>(allergyCreateDto));
                    return new ApiResponse<PatientAllergyReadDto> { Success = true, StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientAllergyReadDto>(allergyCreateDto) };
                }
                return new ApiResponse<PatientAllergyReadDto> { Success = true, Message = "Allergy is already exist for this patient", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientAllergyReadDto>(allergyCreateDto) };
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
        /// update patient allergy by patient Id and Allergy Id
        /// </summary>
        /// <param name="patientAllergy"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PatientAllergyReadDto>> UpdatePatientAllergyAsync(PatientAllergyCreateDto patientAllergy)
        {
            var apiResponse = new ApiResponse<PatientAllergyReadDto>();
            try
            {
                if (_patientAllergyRepository.Queryable().FirstOrDefault(x => x.Id == patientAllergy.Id) != null)
                {
                   var update =  await _patientAllergyRepository.UpdateAsync(_mapper.Map<PatientAllergy>(patientAllergy));
                    return new ApiResponse<PatientAllergyReadDto> { Success = true, Message = "Patient Allergy updated", StatusCode = StatusCodes.Status200OK, Data = _mapper.Map<PatientAllergyReadDto>(update) };
                }
                return new ApiResponse<PatientAllergyReadDto> { Success = false, StatusCode = StatusCodes.Status404NotFound, Data = null, Message = "Record Not Found" };
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
        /// Delete patient allergy by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> DeletePatientAllergyAsync(long id)
        {
            var apiResponse = new ApiResponse<bool>();
            try
            {
                if (await _patientAllergyRepository.Queryable().FirstOrDefaultAsync(x => x.PatientId == id) != null)
                {
                    var patientAllergy = await _patientAllergyRepository.GetByIdAsync(id);
                    if (patientAllergy == null) { return new ApiResponse<bool> { Message = "Alergy ", Success = false, StatusCode = StatusCodes.Status404NotFound, Data = false }; }
                    patientAllergy.IsActive = false;
                    await _patientAllergyRepository.UpdateAsync(patientAllergy);
                    apiResponse = new ApiResponse<bool> { Message = $" Patient Allergy is deleted successfully", Success = true, StatusCode = StatusCodes.Status200OK, Data = true };
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
