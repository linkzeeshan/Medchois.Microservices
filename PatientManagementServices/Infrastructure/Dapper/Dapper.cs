using Dapper;
using Microsoft.Data.SqlClient;
using PatientManagementServices.Application.Core.Background;
using PatientManagementServices.Application.Core.Cache;
using PatientManagementServices.Domain.Interfaces;
using System.Data;

namespace PatientManagementServices.Infrastructure.Dapper
{
    public class Dapper<T> : IDapper<T> where T : class
    {
        private readonly string msg = string.Empty;
        private readonly ILogger<Dapper<T>> _logger;
       // private readonly IBackgroundJob _backgroundJob;
       // private readonly ICacheService _cache;
        private readonly IConfiguration _configuration;

        public Dapper(ILogger<Dapper<T>> logger, /*IBackgroundJob backgroundClient,ICacheService cache,*/  IConfiguration configuration)
        {
            _logger = logger;
            //_backgroundJob = backgroundClient;
           // _cache = cache;
            _configuration = configuration;
        }

        public void Dispose()
        {
        }

        private IDbConnection UserDBConnection(string SQLConnectionstr)
        {
            IDbConnection db = new SqlConnection(SQLConnectionstr);
            if (db.State == ConnectionState.Closed)
            {
                db.Open();
            }

            return db;
        }

        public async Task<int> ExecuteAsync(string Query, DynamicParameters param, CommandType commandType, string SQLConnectionstr)
        {
            if (string.IsNullOrEmpty(SQLConnectionstr) || string.IsNullOrWhiteSpace(SQLConnectionstr))
            {
                SQLConnectionstr = _configuration.GetSection("DefaultConnection").ToString();
            }
            try
            {
                int affectedRows = 0;
                using (IDbConnection db = UserDBConnection(SQLConnectionstr))
                {
                    affectedRows = param == null ? await db.ExecuteAsync(Query, commandType: commandType) : await db.ExecuteAsync(Query, param, commandType: commandType);
                }
                return affectedRows;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ~ " + ex.InnerException);
                throw;
            }
        }

        public async Task<string> ExecuteScalarAsync(string Query, DynamicParameters param, CommandType commandType, string SQLConnectionstr)
        {
            if (string.IsNullOrEmpty(SQLConnectionstr) || string.IsNullOrWhiteSpace(SQLConnectionstr))
            {
                SQLConnectionstr = _configuration.GetSection("DefaultConnection").ToString();
            }
            try
            {
                string data = string.Empty;

                using (IDbConnection db = UserDBConnection(SQLConnectionstr))
                {
                    data = param == null ? await db.ExecuteScalarAsync<string>(Query, commandType: commandType) : await db.ExecuteScalarAsync<string>(Query, param, commandType: commandType);
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ~ " + ex.InnerException);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetDataListAsync<T>(string Query, DynamicParameters param, CommandType commandType, string SQLConnectionstr)
        {
            if (string.IsNullOrEmpty(SQLConnectionstr) || string.IsNullOrWhiteSpace(SQLConnectionstr))
            {
                SQLConnectionstr = _configuration.GetSection("DefaultConnection").ToString();
            }

            try
            {
                using (IDbConnection db = UserDBConnection(SQLConnectionstr))
                {
                    IEnumerable<T> data = param == null ? await db.QueryAsync<T>(Query, commandType: commandType) : await db.QueryAsync<T>(Query, param, commandType: commandType);

                    return data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ~ " + ex.InnerException);
                throw;
            }
        }

        public async Task<DataSet> GetDataSetAsync(string Query, DynamicParameters param, CommandType commandType, string SQLConnectionstr)
        {
            if (string.IsNullOrEmpty(SQLConnectionstr) || string.IsNullOrWhiteSpace(SQLConnectionstr))
            {
                SQLConnectionstr = _configuration.GetSection("DefaultConnection").ToString();
            }
            try
            {
                using (IDbConnection db = UserDBConnection(SQLConnectionstr))
                {
                    IDataReader list = param == null ? await db.ExecuteReaderAsync(Query, commandType: commandType) : await db.ExecuteReaderAsync(Query, param, commandType: commandType);

                    DataSet dataset = ConvertDataReaderToDataSet(list);
                    return dataset;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " ~ " + ex.InnerException);
                throw;
            }
        }

        private DataSet ConvertDataReaderToDataSet(IDataReader data)
        {
            DataSet ds = new DataSet();
            int i = 0;
            while (!data.IsClosed)
            {
                ds.Tables.Add("Table" + i.ToString());
                ds.EnforceConstraints = false;
                ds.Tables[i].Load(data);
                i++;
            }
            return ds;
        }
    }
}
