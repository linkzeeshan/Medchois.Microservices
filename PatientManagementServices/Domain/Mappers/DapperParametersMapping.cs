using Dapper;
using PatientManagementServices.Application.Common;

namespace PatientManagementServices.Domain.Mappers
{
    public static class DapperParametersMapping
    {
     public static DynamicParameters DynamicParameters(ApiBaseSearchRequest dynamicParameters)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PageNumber", dynamicParameters.PageNumber);
            parameters.Add("PageSize", dynamicParameters.PageSize);
            parameters.Add("FirstName", dynamicParameters.FirstName);
            parameters.Add("LastName", dynamicParameters.LastName);
            
        return parameters;
        } 
    }
}
