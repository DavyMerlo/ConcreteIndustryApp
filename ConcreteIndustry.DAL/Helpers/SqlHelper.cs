using System.Data;
using System.Data.SqlClient;

namespace ConcreteIndustry.DAL.Helpers
{
    public static class SqlHelper<T> where T : Enum
    { 
        public static string CreateSelectAllQuery(Enum tableName)
        {
            return $"SELECT * FROM {tableName} WHERE DeletedAt IS NULL";
        }

        public static string CreateSelectByQuery(Enum tableName, T column)
        {
            return $"SELECT * FROM {tableName} WHERE DeletedAt IS NULL AND {column} = @{column}";
        }

        public static string CreateInsertQuery(Enum tableName, T returnId, params T[] columns)
        {
            var columnNames = string.Join(", ", columns.Select(c => GetColumnName(c)));
            var parameterNames = string.Join(", ", columns.Select(c => $"@{c}"));

            return $"INSERT INTO {tableName} ({columnNames}) " +
                   $"OUTPUT INSERTED.{returnId} " + 
                   $"VALUES ({parameterNames})";
        }

        public static string CreateUpdateQuery(Enum tableName, T id, T[] columns)
        {
            var columnUpdates = columns.Select(column => $"{column} = @{column}");
            var setClauseString = string.Join(", ", columnUpdates);

            return $"UPDATE {tableName} SET {setClauseString} " +
                   $"WHERE DeletedAt IS NULL AND {id}  = @{id}";
        }

        public static string CreateDeleteQuery(Enum tableName, T id)
        {
            return $"DELETE FROM {tableName} WHERE DeletedAt IS NULL AND {id} = @{id}"; ;
        }

        public static SqlParameter[] CreateParameters(params (T Column, SqlDbType DbType, object Value)[] paramDetails)
        {
            return paramDetails.Select(pd => CreateParameter(pd.Column, pd.DbType, pd.Value)).ToArray();
        }

        public static SqlParameter CreateOutputParameter(string output, SqlDbType dbType)
        {
            return new SqlParameter
            {
                ParameterName = output,
                SqlDbType = dbType,
                Direction = ParameterDirection.Output
            };
        }

        private static SqlParameter CreateParameter(T column, SqlDbType dbType, object value)
        {
            return new SqlParameter($"@{column}", dbType) { Value = value ?? DBNull.Value };
        }

        private static string GetColumnName(T column)
        {
            return column.ToString();
        }
    }
}

