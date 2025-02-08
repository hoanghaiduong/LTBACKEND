using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;


namespace LTBACKEND.Utils
{
    public class SQLHelper
    {
        private readonly DbConnection _connection;
        private readonly ILogger<SQLHelper> _logger;

        public SQLHelper(DbConnection connection, ILogger<SQLHelper> logger)
        {
            _connection = connection;
            _logger = logger;
        }



        private void LogError(Exception ex, string operation)
        {
            _logger.LogError(ex, $"{operation} failed: {ex.Message}");
        }

        public async Task<long> InsertAsync<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            try
            {
              
                return await _connection.InsertAsync(entity, transaction);
            }
            catch (Exception ex)
            {
                LogError(ex, "Insert");
                return 0;
            }
        }

        public async Task<bool> UpdateAsync<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    await _connection.OpenAsync();

                return await _connection.UpdateAsync(entity, transaction);
            }
            catch (Exception ex)
            {
                LogError(ex, "Update");
                return false;
            }
        }

        public async Task<bool> DeleteAsync<T>(T entity, IDbTransaction transaction = null) where T : class
        {
            try
            {
             
                return await _connection.DeleteAsync(entity, transaction);
            }
            catch (Exception ex)
            {
                LogError(ex, "Delete");
                return false;
            }
        }

        public async Task<IEnumerable<T>> ExecProcedureAsync<T>(string procedureName, object parameters = null, CancellationToken cancellationToken = default)
        {
            try
            {
             

                return await _connection.QueryAsync<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                LogError(ex, "Stored procedure execution");
                throw;
            }
        }

        public async Task<IEnumerable<T>> ExecQueryAsync<T>(string sql, object parameters = null, CancellationToken cancellationToken = default)
        {
            try
            {
              

                return await _connection.QueryAsync<T>(sql, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                LogError(ex, "Query execution");
                throw;
            }
        }

        public async Task<int> ExecNonQueryAsync(string sql, object parameters = null, CancellationToken cancellationToken = default)
        {
            try
            {
              
                return await _connection.ExecuteAsync(sql, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                LogError(ex, "Non-query execution");
                throw;
            }
        }

        public async Task<bool> ExecuteInTransactionAsync(Func<IDbTransaction, Task> action)
        {
            var transaction = await _connection.BeginTransactionAsync();
            try
            {
                await action(transaction);
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                LogError(ex, "Transaction");
                return false;
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}