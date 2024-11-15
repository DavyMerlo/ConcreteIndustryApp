using ConcreteIndustry.DAL.Constants;
using ConcreteIndustry.DAL.Entities;
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
                var query = SqlHelper.CreateSelectAllQuery(Table.Materials);

                return await dataConnection.ExecuteAsync(StoredProcedures.ViewMaterials, reader => new Material
                {
                    Id = reader.GetInt64(Column.Material.MaterialID),
                    Name = reader.GetString(Column.Material.Name),
                    Quantity = reader.GetDecimal(Column.Material.Quantity),
                    PricePerTon = reader.GetDecimal(Column.Material.PricePerTon),
                    SupplierID = reader.GetInt64(Column.Material.SupplierID),
                    CreatedAt = reader.GetDateTime(Column.Material.CreatedAt),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(Column.Material.UpdatedAt),
                    DeletedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(Column.Material.DeletedAt),
                }, null, CommandType.StoredProcedure);
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
                var parameters = SqlHelper.CreateParameters(
                       (Column.Material.MaterialID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(StoredProcedures.ViewMaterialsById, reader =>
                new Material
                {
                    Id = reader.GetInt64(Column.Material.MaterialID),
                    Name = reader.GetString(Column.Material.Name),
                    Quantity = reader.GetDecimal(Column.Material.Quantity),
                    PricePerTon = reader.GetDecimal(Column.Material.PricePerTon),
                    SupplierID = reader.GetInt64(Column.Material.SupplierID),
                    CreatedAt = reader.GetDateTime(Column.Material.CreatedAt),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(Column.Material.UpdatedAt),
                    DeletedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(Column.Material.DeletedAt),
                }, parameters, CommandType.StoredProcedure);

                return result.FirstOrDefault();
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
                var parameters = SqlHelper.CreateParameters(
                     (Column.Material.Name, SqlDbType.NVarChar, material.Name),
                     (Column.Material.Quantity, SqlDbType.Decimal, material.Quantity),
                     (Column.Material.PricePerTon, SqlDbType.Decimal, material.PricePerTon),
                     (Column.Material.SupplierID, SqlDbType.BigInt, material.SupplierID),
                     (Column.Material.MaterialID, SqlDbType.BigInt, material.Id)
                );

                return await dataConnection.ExecuteScalarAsync<int>(
                    StoredProcedures.AddMaterial.ToString(), 
                    parameters, 
                    CommandType.StoredProcedure
                );
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
                    Column.Material.Name,
                    Column.Material.Quantity,
                    Column.Material.PricePerTon,
                    Column.Material.SupplierID
                };

                var query = SqlHelper.CreateUpdateQuery(Table.Materials, Column.Material.MaterialID, columns);

                var parameters = SqlHelper.CreateParameters(
                     (Column.Material.MaterialID, SqlDbType.BigInt, material.Id),
                     (Column.Material.Name, SqlDbType.NVarChar, material.Name),
                     (Column.Material.Quantity, SqlDbType.Decimal, material.Quantity),
                     (Column.Material.PricePerTon, SqlDbType.Decimal, material.PricePerTon),
                     (Column.Material.SupplierID, SqlDbType.BigInt, material.SupplierID)
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
                var query = SqlHelper.CreateDeleteQuery(Table.Materials, Column.Material.MaterialID);

                var parameters = SqlHelper.CreateParameters(
                    (Column.Material.MaterialID, SqlDbType.BigInt, id)
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
