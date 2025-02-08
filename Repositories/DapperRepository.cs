using Microsoft.Data.SqlClient;
using System.Data;

namespace LTBACKEND.Repositories
{
    public class DapperRepository
    {
        private readonly string _connectionString;
      
        public DapperRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        // Thực hiện Stored Procedure với Dapper
        //public async Task<IEnumerable<Product>> GetProductsFromProcedureAsync()
        //{
        //    using var connection = CreateConnection();
        //    return await connection.QueryAsync<Product>("GetAllProducts", commandType: CommandType.StoredProcedure);
        //}

        //// Ví dụ: Chạy query động với Dapper
        //public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        //{
        //    using var connection = CreateConnection();
        //    var query = "SELECT * FROM Products WHERE Category = @Category";
        //    return await connection.QueryAsync<Product>(query, new { Category = category });
        //}

    }
}
