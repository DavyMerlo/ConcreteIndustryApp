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

        Task<(IEnumerable<T> Projects, int TotalCount, int TotalPages, bool HasNext, bool HasPrevious)> ExecuteAsyncWithMetaData<T>(
            string query,
            Func<SqlDataReader, T> readFunc,
            SqlParameter[]? parameters = null,
            CommandType commandType = CommandType.Text);

        Task<int> ExecuteNonQueryAsync(
            string query, 
            SqlParameter[] parameters);

        Task<bool> ExecuteNonQueryAsyncNew(string query,
            SqlParameter[]? parameters = null,
            string? output = null,
            CommandType commandType = CommandType.Text);

        Task<T?> ExecuteScalarAsync<T>(string query,
            SqlParameter[]? parameters = null,
            CommandType commandType = CommandType.Text,
            SqlParameter? output = null);

        Task<T?> ExecuteOutputParameterAsync<T>(string query,
             SqlParameter[]? parameters = null,
             string? output = null,
             SqlDbType? outputParamType = null,
             int? outputParamSize = null,
             CommandType commandType = CommandType.Text);
    }
}
