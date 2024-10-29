using System.Data;
using System.Data.SqlClient;

namespace ConcreteIndustry.DAL.Repositories.Helpers.Interfaces
{
    public interface IDataConnection
    {
        Task<SqlConnection> CreateConnectionAsync();

        Task<IEnumerable<T>> ExecuteAsync<T>(
            string query,
            Func<SqlDataReader, T> readFunc,
            SqlParameter[]? parameters = null,
            CommandType commandType = CommandType.Text);

        Task<int> ExecuteNonQueryAsync(
            string query, 
            SqlParameter[] parameters);

        Task<T?> ExecuteScalarAsync<T>(string query,
            SqlParameter[]? parameters = null,
            CommandType commandType = CommandType.Text,
            SqlParameter? output = null);
    }
}
