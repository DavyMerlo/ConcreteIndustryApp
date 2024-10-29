using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace ConcreteIndustry.DAL.Repositories.Helpers
{
    public class DataConnection : IDataConnection
    {
        private readonly string connectionString;
        private SqlConnection? connection;

        public DataConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<SqlConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<IEnumerable<T>> ExecuteAsync<T>(string query, 
            Func<SqlDataReader, T> readFunc, 
            SqlParameter[]? parameters = null, 
            CommandType commandType = CommandType.Text)
        {
            var results = new List<T>();
            await using (var connection = await CreateConnectionAsync())
            await using (var command = new SqlCommand(query, connection))
            {
                command.CommandType = commandType;
                AddParametersToCommand(command, parameters);
                await using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(readFunc(reader));
                    }
                }
            }
            return results;
        }

        public async Task<int> ExecuteNonQueryAsync(string query, 
            SqlParameter[]? parameters = null)
        {
            await using (var connection = await CreateConnectionAsync())
            await using (var command = new SqlCommand(query, connection))
            {
                AddParametersToCommand(command, parameters);
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<T?> ExecuteScalarAsync<T>(string query, 
            SqlParameter[]? parameters = null, 
            CommandType commandType = CommandType.Text,
            SqlParameter? output = null)
        {
            await using (var connection = await CreateConnectionAsync())
            await using (var command = new SqlCommand(query, connection))
            {
                command.CommandType = commandType;
                AddParametersToCommand(command, parameters);

                if (output != null)
                {
                    command.Parameters.Add(output);
                }

                var result = await command.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                {
                    return default;
                }

                if (output != null && output.Direction == ParameterDirection.Output)
                {
                    return (T)Convert.ChangeType(output.Value, typeof(T));
                }

                return (T)Convert.ChangeType(result, typeof(T));
            }
        }

        private void AddParametersToCommand(SqlCommand command, 
            SqlParameter[]? parameters)
        {
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
        }
    }
}
