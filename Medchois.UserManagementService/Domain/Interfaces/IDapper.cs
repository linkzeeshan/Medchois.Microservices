using Dapper;
using System.Data;

namespace Medchois.UserManagementService.Domain.Interfaces
{
    public interface IDapper<T> : IDisposable
    {
        void Dispose();

        Task<int> ExecuteAsync(string Query, DynamicParameters param, CommandType commandType, string SQLConnectionstr = null);

        Task<string> ExecuteScalarAsync(string Query, DynamicParameters param, CommandType commandType, string SQLConnectionstr = null);

        Task<IEnumerable<T>> GetDataListAsync<T>(string Query, DynamicParameters param, CommandType commandType, string SQLConnectionstr = null);

        Task<DataSet> GetDataSetAsync(string Query, DynamicParameters param, CommandType commandType, string SQLConnectionstr = null);
    }
}
