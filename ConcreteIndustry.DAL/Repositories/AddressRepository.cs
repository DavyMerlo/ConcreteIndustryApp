using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Enums;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using ConcreteIndustry.DAL.Helpers;

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
                var query = SqlHelper<AddressColumn>.CreateSelectAllQuery(TableName.Addresses);

                return await dataConnection.ExecuteAsync(query, reader => new Address
                {
                    Id = reader.GetInt64((int)AddressColumn.AddressID),
                    Street = reader.GetString((int)AddressColumn.Street),
                    HouseNumber = reader.GetString((int)AddressColumn.HouseNumber),
                    BoxNumber = reader.IsDBNull(3) ? null : reader.GetString((int)AddressColumn.BoxNumber),
                    District = reader.GetString((int)AddressColumn.District),
                    Country = reader.GetString((int)AddressColumn.Country),
                    PostalCode = reader.GetString((int)AddressColumn.PostalCode),
                    CreatedAt = reader.GetDateTime((int)AddressColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime((int)AddressColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime((int)AddressColumn.DeletedAt),
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
                var query = SqlHelper<AddressColumn>.CreateSelectByQuery(TableName.Addresses, AddressColumn.AddressID);

                var parameters = SqlHelper<AddressColumn>.CreateParameters(
                   (AddressColumn.AddressID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new Address
                {
                    Id = reader.GetInt64((int)AddressColumn.AddressID),
                    Street = reader.GetString((int)AddressColumn.Street),
                    HouseNumber = reader.GetString((int)AddressColumn.HouseNumber),
                    BoxNumber = reader.IsDBNull(3) ? null : reader.GetString((int)AddressColumn.BoxNumber),
                    District = reader.GetString((int)AddressColumn.District),
                    Country = reader.GetString((int)AddressColumn.Country),
                    PostalCode = reader.GetString((int)AddressColumn.PostalCode),
                    CreatedAt = reader.GetDateTime((int)AddressColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime((int)AddressColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime((int)AddressColumn.DeletedAt),
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
                    AddressColumn.Street,
                    AddressColumn.HouseNumber,
                    AddressColumn.BoxNumber,
                    AddressColumn.District,
                    AddressColumn.Country,
                    AddressColumn.PostalCode,
                };

                var query = SqlHelper<AddressColumn>.CreateInsertQuery(TableName.Addresses, AddressColumn.AddressID, columns);

                var parameters = SqlHelper<AddressColumn>.CreateParameters(
                    (AddressColumn.Street, SqlDbType.NVarChar, address.Street),
                    (AddressColumn.HouseNumber, SqlDbType.NVarChar, address.HouseNumber),
                    (AddressColumn.BoxNumber, SqlDbType.NVarChar, address.BoxNumber ?? (object)DBNull.Value),
                    (AddressColumn.District, SqlDbType.NVarChar, address.District),
                    (AddressColumn.Country, SqlDbType.NVarChar, address.Country),
                    (AddressColumn.PostalCode, SqlDbType.NVarChar, address.PostalCode)
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
                    AddressColumn.Street,
                    AddressColumn.HouseNumber,
                    AddressColumn.BoxNumber,
                    AddressColumn.District,
                    AddressColumn.Country,
                    AddressColumn.PostalCode,
                };

                var query = SqlHelper<AddressColumn>.CreateUpdateQuery(TableName.Addresses, AddressColumn.AddressID, columns);

                var parameters = SqlHelper<AddressColumn>.CreateParameters(
                    (AddressColumn.AddressID, SqlDbType.BigInt, address.Id),
                    (AddressColumn.Street, SqlDbType.NVarChar, address.Street),
                    (AddressColumn.HouseNumber, SqlDbType.NVarChar, address.HouseNumber),
                    (AddressColumn.BoxNumber, SqlDbType.NVarChar, address.BoxNumber ?? (object)DBNull.Value),
                    (AddressColumn.District, SqlDbType.NVarChar, address.District),
                    (AddressColumn.Country, SqlDbType.NVarChar, address.Country),
                    (AddressColumn.PostalCode, SqlDbType.NVarChar, address.PostalCode)
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
                var query = SqlHelper<AddressColumn>.CreateDeleteQuery(TableName.Addresses, AddressColumn.AddressID);

                var parameters = SqlHelper<AddressColumn>.CreateParameters(
                    (AddressColumn.AddressID, SqlDbType.BigInt, id)
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
