using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Enums;
using ConcreteIndustry.DAL.Helpers;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ConcreteIndustry.DAL.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IDataConnection dataConnection;
        private ILogger logger;

        public SupplierRepository(IDataConnection connection, ILogger logger)
        {
            this.dataConnection = connection;
            this.logger = logger;
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersAsync()
        {
            try
            {
                var query = SqlHelper<SupplierColumn>.CreateSelectAllQuery(TableName.Suppliers);

                return await dataConnection.ExecuteAsync(query, reader => new Supplier
                {
                    Id = reader.GetInt64((int)SupplierColumn.SupplierID),
                    Name = reader.GetString((int)SupplierColumn.Name),
                    ContactPerson = reader.GetString((int)SupplierColumn.ContactPerson),
                    PhoneNumber = reader.GetString((int)SupplierColumn.PhoneNumber),
                    Email = reader.GetString((int)SupplierColumn.Email),
                    AddressID = reader.GetInt64((int)SupplierColumn.AddressID),
                    CreatedAt = reader.GetDateTime((int)ProjectColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime((int)SupplierColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime((int)SupplierColumn.DeletedAt),
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(SupplierRepository).ToString()} Get All {nameof(Supplier)} Error", typeof(SupplierRepository));
                throw;
            }
        }

        public async Task<Supplier?> GetSupplierByIdAsync(long id)
        {
            try
            {
                var query = SqlHelper<SupplierColumn>.CreateSelectByQuery(TableName.Suppliers, SupplierColumn.SupplierID);

                var parameters = SqlHelper<SupplierColumn>.CreateParameters(
                   (SupplierColumn.SupplierID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new Supplier
                {
                    Id = reader.GetInt64((int)SupplierColumn.SupplierID),
                    Name = reader.GetString((int)SupplierColumn.Name),
                    ContactPerson = reader.GetString((int)SupplierColumn.ContactPerson),
                    PhoneNumber = reader.GetString((int)SupplierColumn.PhoneNumber),
                    Email = reader.GetString((int)SupplierColumn.Email),
                    AddressID = reader.GetInt64((int)SupplierColumn.AddressID),
                    CreatedAt = reader.GetDateTime((int)ProjectColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime((int)SupplierColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime((int)SupplierColumn.DeletedAt),
                }, parameters);

                return result.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(SupplierRepository).ToString()} Get {nameof(Supplier)} by Id Error", typeof(SupplierRepository));
                throw;
            }
        }

        public async Task<int> AddSupplierAsync(Supplier supplier)
        {
            try
            {
                var columns = new[]
                {
                    SupplierColumn.Name,
                    SupplierColumn.ContactPerson,
                    SupplierColumn.PhoneNumber,
                    SupplierColumn.Email,
                    SupplierColumn.AddressID,
                };

                var query = SqlHelper<SupplierColumn>.CreateInsertQuery(TableName.Suppliers, SupplierColumn.SupplierID, columns);

                var parameters = SqlHelper<SupplierColumn>.CreateParameters(
                     (SupplierColumn.Name, SqlDbType.NVarChar, supplier.Name),
                     (SupplierColumn.ContactPerson, SqlDbType.NVarChar, supplier.ContactPerson),
                     (SupplierColumn.PhoneNumber, SqlDbType.NVarChar, supplier.PhoneNumber),
                     (SupplierColumn.Email, SqlDbType.NVarChar, supplier.Email),
                     (SupplierColumn.AddressID, SqlDbType.BigInt, supplier.AddressID)
                );
                return await dataConnection.ExecuteScalarAsync<int>(query, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(SupplierRepository).ToString()} Add {nameof(Supplier)} Error", typeof(SupplierRepository));
                throw;
            }
        }

        public async Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            try
            {
                var columns = new[]
                {
                    SupplierColumn.Name,
                    SupplierColumn.ContactPerson,
                    SupplierColumn.PhoneNumber,
                    SupplierColumn.Email,
                    SupplierColumn.AddressID,
                };

                var query = SqlHelper<SupplierColumn>.CreateInsertQuery(TableName.Suppliers, SupplierColumn.SupplierID, columns);

                var parameters = SqlHelper<SupplierColumn>.CreateParameters(
                     (SupplierColumn.SupplierID, SqlDbType.BigInt, supplier.Id),
                     (SupplierColumn.Name, SqlDbType.NVarChar, supplier.Name),
                     (SupplierColumn.ContactPerson, SqlDbType.NVarChar, supplier.ContactPerson),
                     (SupplierColumn.PhoneNumber, SqlDbType.NVarChar, supplier.PhoneNumber),
                     (SupplierColumn.Email, SqlDbType.NVarChar, supplier.Email),
                     (SupplierColumn.AddressID, SqlDbType.BigInt, supplier.AddressID)
                );

                int rowsAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(SupplierRepository).ToString()} Update {nameof(Supplier)} Error", typeof(SupplierRepository));
                throw;
            }
        }

        public async Task<bool> DeleteSupplierAsync(long id)
        {
            try
            {
                var query = SqlHelper<SupplierColumn>.CreateDeleteQuery(TableName.Suppliers, SupplierColumn.SupplierID);

                var parameters = SqlHelper<SupplierColumn>.CreateParameters(
                    (SupplierColumn.SupplierID, SqlDbType.BigInt, id)
                );

                int rowAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(SupplierRepository).ToString()} Delete {nameof(Supplier)} Error", typeof(SupplierRepository));
                throw;
            }
        }
    }
}
