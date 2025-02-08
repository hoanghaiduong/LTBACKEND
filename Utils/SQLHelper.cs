using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LTBACKEND.Utils
{
    public class SQLHelper : IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<SQLHelper> _logger;

        public SQLHelper(DatabaseConfig config, ILogger<SQLHelper> logger)
        {
            _connection = new SqlConnection(config.ConnectionString);
            _logger = logger;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        public async Task<long> InsertAsync<T>(T entity) where T : class
        {
            try
            {
                return await _connection.InsertAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Insert failed");
                return 0;
            }
        }

        public async Task<bool> UpdateAsync<T>(T entity) where T : class
        {
            try
            {
                return await _connection.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update failed");
                return false;
            }
        }

        public async Task<bool> DeleteAsync<T>(T entity) where T : class
        {
            try
            {
                return await _connection.DeleteAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete failed");
                return false;
            }
        }

        public async Task<DataTable> ExecProcedureAsync(string procedureName, object parameters = null)
        {
            try
            {
                var reader = await _connection.ExecuteReaderAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var table = new DataTable();
                table.Load(reader);
                return table;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Stored procedure execution failed");
                throw;
            }
        }

        public async Task<IEnumerable<T>> ExecQueryAsync<T>(string sql, object parameters = null)
        {
            try
            {
                return await _connection.QueryAsync<T>(sql, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Query execution failed");
                throw;
            }
        }

        public async Task<int> ExecNonQueryAsync(string sql, object parameters = null)
        {
            try
            {
                return await _connection.ExecuteAsync(sql, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-query execution failed");
                throw;
            }
        }
    }
}
