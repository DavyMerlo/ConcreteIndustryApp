using System.Data;
using System.Data.SqlClient;

namespace ConcreteIndustry.DAL.Helpers
{
    public static class SqlHelper
    { 
        public static string CreateSelectAllQuery(string tableName)
        {
            return $"SELECT * FROM {tableName} WHERE DeletedAt IS NULL";
        }

        public static string CreateSelectByQuery(string tableName, string column)
        {
            return $"SELECT * FROM {tableName} WHERE DeletedAt IS NULL AND {column} = @{column}";
        }

        public static string CreateInsertQuery(string tableName, string returnId, params string[] columns)
        {
            var columnNames = string.Join(", ", columns.Select(c => GetColumnName(c)));
            var parameterNames = string.Join(", ", columns.Select(c => $"@{c}"));

            return $"INSERT INTO {tableName} ({columnNames}) " +
                   $"OUTPUT INSERTED.{returnId} " + 
                   $"VALUES ({parameterNames})";
        }

        public static string CreateUpdateQuery(string tableName, string id, string[] columns)
        {
            var columnUpdates = columns.Select(column => $"{column} = @{column}");
            var setClauseString = string.Join(", ", columnUpdates);

            return $"UPDATE {tableName} SET {setClauseString} " +
                   $"WHERE DeletedAt IS NULL AND {id}  = @{id}";
        }

        public static string CreateDeleteQuery(string tableName, string id)
        {
            return $"DELETE FROM {tableName} WHERE DeletedAt IS NULL AND {id} = @{id}"; ;
        }

        public static SqlParameter[] CreateParameters(params (string name, SqlDbType DbType, object Value)[] paramDetails)
        {
            return paramDetails.Select(pd => CreateParameter(pd.name, pd.DbType, pd.Value)).ToArray();
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

        private static SqlParameter CreateParameter(string name, SqlDbType dbType, object value)
        {
            return new SqlParameter($"@{name}", dbType) { Value = value ?? DBNull.Value };
        }

        private static string GetColumnName(string column)
        {
            return column.ToString();
        }
    }
}

