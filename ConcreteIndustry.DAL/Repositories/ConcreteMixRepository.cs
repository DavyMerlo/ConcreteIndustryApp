using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Enums;
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
                var query = SqlHelper<ConcreteMixColumn>.CreateSelectAllQuery(TableName.ConcreteMixes);

                return await dataConnection.ExecuteAsync(query, reader => new ConcreteMix
                {
                    Id = reader.GetInt64((int)ConcreteMixColumn.ConcreteMixID),
                    Name = reader.GetString((int)ConcreteMixColumn.Name),
                    StrengthClass = reader.GetString((int)ConcreteMixColumn.StrengthClass),
                    MaxAggregateSize = reader.IsDBNull((int)ConcreteMixColumn.MaxAggregateSize) ? (decimal?)null : reader.GetDecimal((int)ConcreteMixColumn.MaxAggregateSize),
                    WaterCementRatio = reader.IsDBNull((int)ConcreteMixColumn.WaterCementRatio) ? (decimal?)null : reader.GetDecimal((int)ConcreteMixColumn.WaterCementRatio),
                    Application = reader.GetString((int)ConcreteMixColumn.Application),
                    PricePerM3 = reader.GetDecimal((int)ConcreteMixColumn.PricePerM3),
                    CreatedAt = reader.GetDateTime((int)ConcreteMixColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull((int)ConcreteMixColumn.UpdatedAt) ? (DateTime?)null : reader.GetDateTime((int)ConcreteMixColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull((int)ConcreteMixColumn.DeletedAt) ? (DateTime?)null : reader.GetDateTime((int)ConcreteMixColumn.DeletedAt),
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
                var query = SqlHelper<ConcreteMixColumn>.CreateSelectByQuery(TableName.ConcreteMixes, ConcreteMixColumn.ConcreteMixID);

                var parameters = SqlHelper<ConcreteMixColumn>.CreateParameters(
                    (ConcreteMixColumn.ConcreteMixID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new ConcreteMix
                {
                    Id = reader.GetInt64((int)ConcreteMixColumn.ConcreteMixID),
                    Name = reader.GetString((int)ConcreteMixColumn.Name),
                    StrengthClass = reader.GetString((int)ConcreteMixColumn.StrengthClass),
                    MaxAggregateSize = reader.IsDBNull((int)ConcreteMixColumn.MaxAggregateSize) ? (decimal?)null : reader.GetDecimal((int)ConcreteMixColumn.MaxAggregateSize),
                    WaterCementRatio = reader.IsDBNull((int)ConcreteMixColumn.WaterCementRatio) ? (decimal?)null : reader.GetDecimal((int)ConcreteMixColumn.WaterCementRatio),
                    Application = reader.GetString((int)ConcreteMixColumn.Application),
                    PricePerM3 = reader.GetDecimal((int)ConcreteMixColumn.PricePerM3),
                    CreatedAt = reader.GetDateTime((int)ConcreteMixColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull((int)ConcreteMixColumn.UpdatedAt) ? (DateTime?)null : reader.GetDateTime((int)ConcreteMixColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull((int)ConcreteMixColumn.DeletedAt) ? (DateTime?)null : reader.GetDateTime((int)ConcreteMixColumn.DeletedAt),
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
                    ConcreteMixColumn.Name,
                    ConcreteMixColumn.StrengthClass,
                    ConcreteMixColumn.MaxAggregateSize,
                    ConcreteMixColumn.WaterCementRatio,
                    ConcreteMixColumn.Application,
                    ConcreteMixColumn.PricePerM3,
                };

                var query = SqlHelper<ConcreteMixColumn>.CreateInsertQuery(TableName.ConcreteMixes, ConcreteMixColumn.ConcreteMixID, columns);

                var parameters = SqlHelper<ConcreteMixColumn>.CreateParameters(
                    (ConcreteMixColumn.Name, SqlDbType.NVarChar, concreteMix.Name),
                    (ConcreteMixColumn.StrengthClass, SqlDbType.NVarChar, concreteMix.StrengthClass),
                    (ConcreteMixColumn.MaxAggregateSize, SqlDbType.Decimal, concreteMix.MaxAggregateSize ?? (object)DBNull.Value),
                    (ConcreteMixColumn.WaterCementRatio, SqlDbType.Decimal, concreteMix.WaterCementRatio ?? (object)DBNull.Value),
                    (ConcreteMixColumn.Application, SqlDbType.NVarChar, concreteMix.Application),
                    (ConcreteMixColumn.PricePerM3, SqlDbType.Decimal, concreteMix.PricePerM3)
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
                    ConcreteMixColumn.Name,
                    ConcreteMixColumn.StrengthClass,
                    ConcreteMixColumn.MaxAggregateSize,
                    ConcreteMixColumn.WaterCementRatio,
                    ConcreteMixColumn.Application,
                    ConcreteMixColumn.PricePerM3,
                };

                var query = SqlHelper<ConcreteMixColumn>.CreateUpdateQuery(TableName.ConcreteMixes, ConcreteMixColumn.ConcreteMixID, columns);

                var parameters = SqlHelper<ConcreteMixColumn>.CreateParameters(
                    (ConcreteMixColumn.ConcreteMixID, SqlDbType.BigInt, concreteMix.Id),
                    (ConcreteMixColumn.Name, SqlDbType.NVarChar, concreteMix.Name),
                    (ConcreteMixColumn.StrengthClass, SqlDbType.NVarChar, concreteMix.StrengthClass),
                    (ConcreteMixColumn.MaxAggregateSize, SqlDbType.Decimal, concreteMix.MaxAggregateSize ?? (object)DBNull.Value),
                    (ConcreteMixColumn.WaterCementRatio, SqlDbType.Decimal, concreteMix.WaterCementRatio ?? (object)DBNull.Value),
                    (ConcreteMixColumn.Application, SqlDbType.NVarChar, concreteMix.Application),
                    (ConcreteMixColumn.PricePerM3, SqlDbType.Decimal, concreteMix.PricePerM3)
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
                var query = SqlHelper<ConcreteMixColumn>.CreateDeleteQuery(TableName.ConcreteMixes, ConcreteMixColumn.ConcreteMixID);

                var parameters = SqlHelper<ConcreteMixColumn>.CreateParameters(
                    (ConcreteMixColumn.ConcreteMixID, SqlDbType.BigInt, id)
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
