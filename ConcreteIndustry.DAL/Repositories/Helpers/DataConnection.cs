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

        public async Task<(IEnumerable<T> Projects, int TotalCount, int TotalPages, bool HasNext, bool HasPrevious)> ExecuteAsyncWithMetaData<T>(
            string query,
            Func<SqlDataReader, T> readFunc,
            SqlParameter[]? parameters = null,
            CommandType commandType = CommandType.Text)
        {
            var results = new List<T>();
            int totalCount = 0;
            int totalPages = 0;
            bool hasNext = false;
            bool hasPrevious = false;
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

                    if (await reader.NextResultAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            totalCount = reader.GetInt32(reader.GetOrdinal("TotalItems"));
                            totalPages = reader.GetInt32(reader.GetOrdinal("TotalPages"));
                            hasNext = reader.GetBoolean(reader.GetOrdinal("HasNext"));
                            hasPrevious = reader.GetBoolean(reader.GetOrdinal("HasPrevious"));
                        }
                    }
                }
            }
            return (results, totalCount, totalPages, hasNext, hasPrevious);
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

        public async Task<bool> ExecuteNonQueryAsyncNew(string query,
            SqlParameter[]? parameters = null,
            string? output = null,
            CommandType commandType = CommandType.Text)
        {
            SqlParameter? outputParam = null;
            if (!string.IsNullOrEmpty(output))
            {
                outputParam = new SqlParameter($"@{output}", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            }

            await using (var connection = await CreateConnectionAsync())
            await using (var command = new SqlCommand(query, connection))
            {
                command.CommandType = commandType;

                AddParametersToCommand(command, parameters);

                if (outputParam != null)
                {
                    command.Parameters.Add(outputParam);
                }

                int affectedRows = await command.ExecuteNonQueryAsync();

                if (outputParam != null)
                {
                    bool isSuccess = (bool)outputParam.Value;
                    return isSuccess;
                }
                return affectedRows > 0;
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

        public async Task<T?> ExecuteOutputParameterAsync<T>(string query,
             SqlParameter[]? parameters = null,
             string? output = null,
             SqlDbType? outputParamType = null, 
             int? outputParamSize = null,    
             CommandType commandType = CommandType.Text)
        {
            SqlParameter? outputParam = null;

            if (!string.IsNullOrEmpty(output) && outputParamType.HasValue)
            {
                outputParam = new SqlParameter($"@{output}", outputParamType.Value)
                {
                    Direction = ParameterDirection.Output
                };

                if (outputParamSize.HasValue)
                {
                    outputParam.Size = outputParamSize.Value;
                }
            }

            await using (var connection = await CreateConnectionAsync())
            await using (var command = new SqlCommand(query, connection))
            {
                command.CommandType = commandType;
                AddParametersToCommand(command, parameters);

                if (outputParam != null)
                {
                    command.Parameters.Add(outputParam);
                }

                await command.ExecuteNonQueryAsync();

                if (outputParam != null && outputParam.Value != DBNull.Value)
                {
                    return (T)Convert.ChangeType(outputParam.Value, typeof(T));
                }
                return default;
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
