using ConcreteIndustry.DAL.Constants;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Helpers;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ConcreteIndustry.DAL.Repositories
{
    public class ConcreteMixRepository : IConcreteMixRepository
    {
        private readonly IDataConnection dataConnection;
        private readonly ILogger logger;

        public ConcreteMixRepository(IDataConnection dataConnection, ILogger logger)
        {
            this.dataConnection = dataConnection;
            this.logger = logger;
        }

        public async Task<IEnumerable<ConcreteMix>> GetConcreteMixesAsync()
        {
            try
            {
                var query = SqlHelper.CreateSelectAllQuery(Table.ConcreteMixes);

                return await dataConnection.ExecuteAsync(query, reader => new ConcreteMix
                {
                    Id = reader.GetInt64(Column.ConcreteMix.ConcreteMixID),
                    Name = reader.GetString(Column.ConcreteMix.Name),
                    StrengthClass = reader.GetString(Column.ConcreteMix.StrengthClass),
                    MaxAggregateSize = reader.IsDBNull(Column.ConcreteMix.MaxAggregateSize) ? (decimal?)null : reader.GetDecimal(Column.ConcreteMix.MaxAggregateSize),
                    WaterCementRatio = reader.IsDBNull(Column.ConcreteMix.WaterCementRatio) ? (decimal?)null : reader.GetDecimal(Column.ConcreteMix.WaterCementRatio),
                    Application = reader.GetString(Column.ConcreteMix.Application),
                    PricePerM3 = reader.GetDecimal(Column.ConcreteMix.PricePerM3),
                    CreatedAt = reader.GetDateTime(Column.ConcreteMix.CreatedAt),
                    UpdatedAt = reader.IsDBNull(Column.ConcreteMix.UpdatedAt) ? (DateTime?)null : reader.GetDateTime(Column.ConcreteMix.UpdatedAt),
                    DeletedAt = reader.IsDBNull(Column.ConcreteMix.DeletedAt) ? (DateTime?)null : reader.GetDateTime(Column.ConcreteMix.DeletedAt),
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ConcreteMixRepository).ToString()} Get All {nameof(ConcreteMix)} Error", typeof(ConcreteMixRepository));
                throw;
            }
        }

        public async Task<ConcreteMix?> GetConcreteMixByIdAsync(long id)
        {
            try
            {
                var query = SqlHelper.CreateSelectByQuery(Table.ConcreteMixes, Column.ConcreteMix.ConcreteMixID);

                var parameters = SqlHelper.CreateParameters(
                    (Column.ConcreteMix.ConcreteMixID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new ConcreteMix
                {
                    Id = reader.GetInt64(Column.ConcreteMix.ConcreteMixID),
                    Name = reader.GetString(Column.ConcreteMix.Name),
                    StrengthClass = reader.GetString(Column.ConcreteMix.StrengthClass),
                    MaxAggregateSize = reader.IsDBNull(Column.ConcreteMix.MaxAggregateSize) ? (decimal?)null : reader.GetDecimal(Column.ConcreteMix.MaxAggregateSize),
                    WaterCementRatio = reader.IsDBNull(Column.ConcreteMix.WaterCementRatio) ? (decimal?)null : reader.GetDecimal(Column.ConcreteMix.WaterCementRatio),
                    Application = reader.GetString(Column.ConcreteMix.Application),
                    PricePerM3 = reader.GetDecimal(Column.ConcreteMix.PricePerM3),
                    CreatedAt = reader.GetDateTime(Column.ConcreteMix.CreatedAt),
                    UpdatedAt = reader.IsDBNull(Column.ConcreteMix.UpdatedAt) ? (DateTime?)null : reader.GetDateTime(Column.ConcreteMix.UpdatedAt),
                    DeletedAt = reader.IsDBNull(Column.ConcreteMix.DeletedAt) ? (DateTime?)null : reader.GetDateTime(Column.ConcreteMix.DeletedAt),
                }, parameters);

                return result.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ConcreteMixRepository).ToString()} Get {nameof(ConcreteMix)} by Id Error", typeof(ConcreteMixRepository));
                throw;
            }
        }

        public async Task<int> AddConcreteMixAsync(ConcreteMix concreteMix)
        {
            try
            {
                var columns = new[]
                {
                    Column.ConcreteMix.Name,
                    Column.ConcreteMix.StrengthClass,
                    Column.ConcreteMix.MaxAggregateSize,
                    Column.ConcreteMix.WaterCementRatio,
                    Column.ConcreteMix.Application,
                    Column.ConcreteMix.PricePerM3,
                };

                var query = SqlHelper.CreateInsertQuery(Table.ConcreteMixes, Column.ConcreteMix.ConcreteMixID, columns);

                var parameters = SqlHelper.CreateParameters(
                    (Column.ConcreteMix.Name, SqlDbType.NVarChar, concreteMix.Name),
                    (Column.ConcreteMix.StrengthClass, SqlDbType.NVarChar, concreteMix.StrengthClass),
                    (Column.ConcreteMix.MaxAggregateSize, SqlDbType.Decimal, concreteMix.MaxAggregateSize ?? (object)DBNull.Value),
                    (Column.ConcreteMix.WaterCementRatio, SqlDbType.Decimal, concreteMix.WaterCementRatio ?? (object)DBNull.Value),
                    (Column.ConcreteMix.Application, SqlDbType.NVarChar, concreteMix.Application),
                    (Column.ConcreteMix.PricePerM3, SqlDbType.Decimal, concreteMix.PricePerM3)
                );

                return await dataConnection.ExecuteScalarAsync<int>(query, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ConcreteMixRepository).ToString()} Add {nameof(ConcreteMix)} Error", typeof(ConcreteMixRepository));
                throw;
            }
        }

        public async Task<bool> UpdateConcreteMixAsync(ConcreteMix concreteMix)
        {
            try
            {
                var columns = new[]
               {
                    Column.ConcreteMix.Name,
                    Column.ConcreteMix.StrengthClass,
                    Column.ConcreteMix.MaxAggregateSize,
                    Column.ConcreteMix.WaterCementRatio,
                    Column.ConcreteMix.Application,
                    Column.ConcreteMix.PricePerM3,
                };

                var query = SqlHelper.CreateUpdateQuery(Table.ConcreteMixes, Column.ConcreteMix.ConcreteMixID, columns);

                var parameters = SqlHelper.CreateParameters(
                    (Column.ConcreteMix.ConcreteMixID, SqlDbType.BigInt, concreteMix.Id),
                    (Column.ConcreteMix.Name, SqlDbType.NVarChar, concreteMix.Name),
                    (Column.ConcreteMix.StrengthClass, SqlDbType.NVarChar, concreteMix.StrengthClass),
                    (Column.ConcreteMix.MaxAggregateSize, SqlDbType.Decimal, concreteMix.MaxAggregateSize ?? (object)DBNull.Value),
                    (Column.ConcreteMix.WaterCementRatio, SqlDbType.Decimal, concreteMix.WaterCementRatio ?? (object)DBNull.Value),
                    (Column.ConcreteMix.Application, SqlDbType.NVarChar, concreteMix.Application),
                    (Column.ConcreteMix.PricePerM3, SqlDbType.Decimal, concreteMix.PricePerM3)
                );

                int rowsAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ConcreteMixRepository).ToString()} Update {nameof(ConcreteMix)} Error", typeof(ConcreteMixRepository));
                throw;
            }
        }

        public async Task<bool> DeleteConcreteMixAsync(long id)
        {
            try
            {
                var query = SqlHelper.CreateDeleteQuery(Table.ConcreteMixes, Column.ConcreteMix.ConcreteMixID);

                var parameters = SqlHelper.CreateParameters(
                    (Column.ConcreteMix.ConcreteMixID, SqlDbType.BigInt, id)
                );   

                int rowAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ConcreteMixRepository).ToString()} Delete {nameof(ConcreteMix)} Error", typeof(ConcreteMixRepository));
                throw;
            }
        }
    }
}
