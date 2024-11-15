using ConcreteIndustry.DAL.Constants;
using ConcreteIndustry.DAL.Entities;
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
                var query = SqlHelper.CreateSelectAllQuery(Table.Suppliers);

                return await dataConnection.ExecuteAsync(query, reader => new Supplier
                {
                    Id = reader.GetInt64(Column.Supplier.SupplierID),
                    Name = reader.GetString(Column.Supplier.Name),
                    ContactPerson = reader.GetString(Column.Supplier.ContactPerson),
                    PhoneNumber = reader.GetString(Column.Supplier.PhoneNumber),
                    Email = reader.GetString(Column.Supplier.Email),
                    AddressID = reader.GetInt64(Column.Supplier.AddressID),
                    CreatedAt = reader.GetDateTime(Column.Supplier.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(Column.Supplier.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(Column.Supplier.DeletedAt),
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
                var query = SqlHelper.CreateSelectByQuery(Table.Suppliers, Column.Supplier.SupplierID);

                var parameters = SqlHelper.CreateParameters(
                   (Column.Supplier.SupplierID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new Supplier
                {
                    Id = reader.GetInt64(Column.Supplier.SupplierID),
                    Name = reader.GetString(Column.Supplier.Name),
                    ContactPerson = reader.GetString(Column.Supplier.ContactPerson),
                    PhoneNumber = reader.GetString(Column.Supplier.PhoneNumber),
                    Email = reader.GetString(Column.Supplier.Email),
                    AddressID = reader.GetInt64(Column.Supplier.AddressID),
                    CreatedAt = reader.GetDateTime(Column.Supplier.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(Column.Supplier.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(Column.Supplier.DeletedAt),
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
                    Column.Supplier.Name,
                    Column.Supplier.ContactPerson,
                    Column.Supplier.PhoneNumber,
                    Column.Supplier.Email,
                    Column.Supplier.AddressID,
                };

                var query = SqlHelper.CreateInsertQuery(Table.Suppliers, Column.Supplier.SupplierID, columns);

                var parameters = SqlHelper.CreateParameters(
                     (Column.Supplier.Name, SqlDbType.NVarChar, supplier.Name),
                     (Column.Supplier.ContactPerson, SqlDbType.NVarChar, supplier.ContactPerson),
                     (Column.Supplier.PhoneNumber, SqlDbType.NVarChar, supplier.PhoneNumber),
                     (Column.Supplier.Email, SqlDbType.NVarChar, supplier.Email),
                     (Column.Supplier.AddressID, SqlDbType.BigInt, supplier.AddressID)
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
                    Column.Supplier.Name,
                    Column.Supplier.ContactPerson,
                    Column.Supplier.PhoneNumber,
                    Column.Supplier.Email,
                    Column.Supplier.AddressID,
                };

                var query = SqlHelper.CreateInsertQuery(Table.Suppliers, Column.Supplier.SupplierID, columns);

                var parameters = SqlHelper.CreateParameters(
                     (Column.Supplier.SupplierID, SqlDbType.BigInt, supplier.Id),
                     (Column.Supplier.Name, SqlDbType.NVarChar, supplier.Name),
                     (Column.Supplier.ContactPerson, SqlDbType.NVarChar, supplier.ContactPerson),
                     (Column.Supplier.PhoneNumber, SqlDbType.NVarChar, supplier.PhoneNumber),
                     (Column.Supplier.Email, SqlDbType.NVarChar, supplier.Email),
                     (Column.Supplier.AddressID, SqlDbType.BigInt, supplier.AddressID)
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
                var query = SqlHelper.CreateDeleteQuery(Table.Suppliers, Column.Supplier.SupplierID);

                var parameters = SqlHelper.CreateParameters(
                    (Column.Supplier.SupplierID, SqlDbType.BigInt, id)
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
