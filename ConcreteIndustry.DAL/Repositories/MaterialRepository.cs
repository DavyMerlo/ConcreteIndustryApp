using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Enums;
using ConcreteIndustry.DAL.Helpers;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ConcreteIndustry.DAL.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly IDataConnection dataConnection;
        private ILogger logger;

        public MaterialRepository(IDataConnection connection, ILogger logger)
        {
            this.dataConnection = connection;
            this.logger = logger;
        }

        public async Task<IEnumerable<Material>> GetMaterialsAsync()
        {
            try
            {
                var query = SqlHelper<MaterialColumn>.CreateSelectAllQuery(TableName.Materials);

                return await dataConnection.ExecuteAsync(query, reader => new Material
                {
                    Id = reader.GetInt64((int)MaterialColumn.MaterialID),
                    Name = reader.GetString((int)MaterialColumn.Name),
                    Quantity = reader.GetDecimal((int)MaterialColumn.Quantity),
                    PricePerTon = reader.GetDecimal((int)MaterialColumn.PricePerTon),
                    SupplierID = reader.GetInt64((int)MaterialColumn.SupplierID),
                    CreatedAt = reader.GetDateTime((int)MaterialColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime((int)MaterialColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(7) ? null : reader.GetDateTime((int)MaterialColumn.DeletedAt),
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(MaterialRepository).ToString()} Get All {nameof(Material)} Error", typeof(MaterialRepository));
                throw;
            }
        }

        public async Task<Material?> GetMaterialByIdAsync(long id)
        {
            try
            {
                var query = SqlHelper<MaterialColumn>.CreateSelectByQuery(TableName.Materials, MaterialColumn.MaterialID);

                var parameters = SqlHelper<MaterialColumn>.CreateParameters(
                   (MaterialColumn.MaterialID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new Material
                {
                    Id = reader.GetInt64((int)MaterialColumn.MaterialID),
                    Name = reader.GetString((int)MaterialColumn.Name),
                    Quantity = reader.GetDecimal((int)MaterialColumn.Quantity),
                    PricePerTon = reader.GetDecimal((int)MaterialColumn.PricePerTon),
                    SupplierID = reader.GetInt64((int)MaterialColumn.SupplierID),
                    CreatedAt = reader.GetDateTime((int)MaterialColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime((int)MaterialColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(7) ? null : reader.GetDateTime((int)MaterialColumn.DeletedAt),
                }, parameters);

                return result.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(MaterialRepository).ToString()} Get {nameof(Material)} by Id Error", typeof(MaterialRepository));
                throw;
            }
        }

        public async Task<int> AddMaterialAsync(Material material)
        {
            try
            {
                var columns = new[]
                {
                    MaterialColumn.Name,
                    MaterialColumn.Quantity,
                    MaterialColumn.PricePerTon,
                    MaterialColumn.SupplierID
                };

                var query = SqlHelper<MaterialColumn>.CreateInsertQuery(TableName.Materials, MaterialColumn.MaterialID, columns);

                var parameters = SqlHelper<MaterialColumn>.CreateParameters(
                     (MaterialColumn.Name, SqlDbType.NVarChar, material.Name),
                     (MaterialColumn.Quantity, SqlDbType.Decimal, material.Quantity),
                     (MaterialColumn.PricePerTon, SqlDbType.Decimal, material.PricePerTon),
                     (MaterialColumn.SupplierID, SqlDbType.BigInt, material.SupplierID)
                );
                return await dataConnection.ExecuteScalarAsync<int>(query, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(MaterialRepository).ToString()} Add {nameof(Material)} Error", typeof(MaterialRepository));
                throw;
            }
        }

        public async Task<bool> UpdateMaterialAsync(Material material)
        {
            try
            {
                var columns = new[]
                {
                    MaterialColumn.Name,
                    MaterialColumn.Quantity,
                    MaterialColumn.PricePerTon,
                    MaterialColumn.SupplierID
                };

                var query = SqlHelper<MaterialColumn>.CreateUpdateQuery(TableName.Materials, MaterialColumn.MaterialID, columns);

                var parameters = SqlHelper<MaterialColumn>.CreateParameters(
                     (MaterialColumn.MaterialID, SqlDbType.BigInt, material.Id),
                     (MaterialColumn.Name, SqlDbType.NVarChar, material.Name),
                     (MaterialColumn.Quantity, SqlDbType.Decimal, material.Quantity),
                     (MaterialColumn.PricePerTon, SqlDbType.Decimal, material.PricePerTon),
                     (MaterialColumn.SupplierID, SqlDbType.BigInt, material.SupplierID)
                );
                int rowsAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(MaterialRepository).ToString()} Update {nameof(Material)} Error", typeof(MaterialRepository));
                throw;
            }
        }

        public async Task<bool> DeleteMaterialAsync(long id)
        {
            try
            {
                var query = SqlHelper<MaterialColumn>.CreateDeleteQuery(TableName.Materials, MaterialColumn.MaterialID);

                var parameters = SqlHelper<MaterialColumn>.CreateParameters(
                    (MaterialColumn.MaterialID, SqlDbType.BigInt, id)
                );

                int rowAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(MaterialRepository).ToString()} Delete {nameof(Material)} Error", typeof(MaterialRepository));
                throw;
            }

        }
    }
}
