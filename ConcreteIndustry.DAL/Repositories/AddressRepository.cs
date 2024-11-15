using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using ConcreteIndustry.DAL.Helpers;
using ConcreteIndustry.DAL.Constants;

namespace ConcreteIndustry.DAL.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IDataConnection dataConnection;
        private ILogger logger;

        public AddressRepository(IDataConnection dataConnection, ILogger logger)
        {
            this.dataConnection = dataConnection;
            this.logger = logger;
        }

        public async Task<IEnumerable<Address>> GetAddressesAsync()
        {
            try
            {
                var query = SqlHelper.CreateSelectAllQuery(Table.Addresses);

                return await dataConnection.ExecuteAsync(query, reader => new Address
                {
                    Id = reader.GetInt64(Column.Address.AddressID),
                    Street = reader.GetString(Column.Address.Street),
                    HouseNumber = reader.GetString(Column.Address.HouseNumber),
                    BoxNumber = reader.IsDBNull(3) ? null : reader.GetString(Column.Address.BoxNumber),
                    District = reader.GetString(Column.Address.District),
                    Country = reader.GetString(Column.Address.Country),
                    PostalCode = reader.GetString(Column.Address.PostalCode),
                    CreatedAt = reader.GetDateTime(Column.Address.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(Column.Address.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(Column.Address.DeletedAt),
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AddressRepository).ToString()} Get All {nameof(Address)} Error", typeof(AddressRepository));
                throw;
            }
        }

        public async Task<Address?> GetAddressByIdAsync(long id)
        {
            try
            {
                var query = SqlHelper.CreateSelectByQuery(Table.Addresses, Column.Address.AddressID);

                var parameters = SqlHelper.CreateParameters(
                   (Column.Address.AddressID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new Address
                {
                    Id = reader.GetInt64(Column.Address.AddressID),
                    Street = reader.GetString(Column.Address.Street),
                    HouseNumber = reader.GetString(Column.Address.HouseNumber),
                    BoxNumber = reader.IsDBNull(3) ? null : reader.GetString(Column.Address.BoxNumber),
                    District = reader.GetString(Column.Address.District),
                    Country = reader.GetString(Column.Address.Country),
                    PostalCode = reader.GetString(Column.Address.PostalCode),
                    CreatedAt = reader.GetDateTime(Column.Address.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(Column.Address.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(Column.Address.DeletedAt),
                }, parameters);

                return result.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AddressRepository).ToString()} Get {nameof(Address)} by Id Error", typeof(AddressRepository));
                throw;
            }
        }

        public async Task<int> AddAddressAsync(Address address)
        {
            try
            {
                var columns = new[]
                {
                    Column.Address.Street,
                    Column.Address.HouseNumber,
                    Column.Address.BoxNumber,
                    Column.Address.District,
                    Column.Address.Country,
                    Column.Address.PostalCode,
                };

                var query = SqlHelper.CreateInsertQuery(Table.Addresses, Column.Address.AddressID, columns);

                var parameters = SqlHelper.CreateParameters(
                    (Column.Address.Street, SqlDbType.NVarChar, address.Street),
                    (Column.Address.HouseNumber, SqlDbType.NVarChar, address.HouseNumber),
                    (Column.Address.BoxNumber, SqlDbType.NVarChar, address.BoxNumber ?? (object)DBNull.Value),
                    (Column.Address.District, SqlDbType.NVarChar, address.District),
                    (Column.Address.Country, SqlDbType.NVarChar, address.Country),
                    (Column.Address.PostalCode, SqlDbType.NVarChar, address.PostalCode)
                );

                return await dataConnection.ExecuteScalarAsync<int>(query, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AddressRepository).ToString()} Add {nameof(Address)} Error", typeof(Address));
                throw;
            }
        }

        public async Task<bool> UpdateAddressAsync(Address address)
        {
            try
            {
                var columns = new[]
               {
                    Column.Address.Street,
                    Column.Address.HouseNumber,
                    Column.Address.BoxNumber,
                    Column.Address.District,
                    Column.Address.Country,
                    Column.Address.PostalCode,
                };

                var query = SqlHelper.CreateUpdateQuery(Table.Addresses, Column.Address.AddressID, columns);

                var parameters = SqlHelper.CreateParameters(
                    (Column.Address.AddressID, SqlDbType.BigInt, address.Id),
                    (Column.Address.Street, SqlDbType.NVarChar, address.Street),
                    (Column.Address.HouseNumber, SqlDbType.NVarChar, address.HouseNumber),
                    (Column.Address.BoxNumber, SqlDbType.NVarChar, address.BoxNumber ?? (object)DBNull.Value),
                    (Column.Address.District, SqlDbType.NVarChar, address.District),
                    (Column.Address.Country, SqlDbType.NVarChar, address.Country),
                    (Column.Address.PostalCode, SqlDbType.NVarChar, address.PostalCode)
                );

                int rowsAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AddressRepository).ToString()} Update {nameof(Address)} Error", typeof(AddressRepository));
                throw;
            }
        }

        public async Task<bool> DeleteAddressAsync(long id)
        {
            try
            {
                var query = SqlHelper.CreateDeleteQuery(Table.Addresses, Column.Address.AddressID);

                var parameters = SqlHelper.CreateParameters(
                    (Column.Address.AddressID, SqlDbType.BigInt, id)
                );

                int rowAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AddressRepository).ToString()} Delete {nameof(Address)} Error", typeof(AddressRepository));
                throw;
            }
        }
    }
}
