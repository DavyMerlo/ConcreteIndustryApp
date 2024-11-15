using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using ConcreteIndustry.DAL.Helpers;
using ConcreteIndustry.DAL.Constants;

namespace ConcreteIndustry.DAL.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IDataConnection dataConnection;
        private ILogger logger;

        public ClientRepository(IDataConnection dataConnection, ILogger logger)
        {
            this.dataConnection = dataConnection;
            this.logger = logger;
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            try
            {
                var query = SqlHelper.CreateSelectAllQuery(Table.Clients);

                return await dataConnection.ExecuteAsync(query, reader => new Client
                {
                    Id = reader.GetInt64(Column.Client.ClientID),
                    CompanyName = reader.GetString(Column.Client.CompanyName),
                    ContactPerson = reader.GetString(Column.Client.ContactPerson),
                    PhoneNumber = reader.GetString(Column.Client.PhoneNumber),
                    Email = reader.GetString(Column.Client.Email),
                    AddressID = reader.GetInt64(Column.Client.AddressID),
                    CreatedAt = reader.GetDateTime(Column.Client.CreatedAt),
                    UpdatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(Column.Client.UpdatedAt),
                    DeletedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(Column.Client.DeletedAt),
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ClientRepository).ToString()} Get All {nameof(Client)} Error", typeof(ClientRepository));
                throw;
            }
        }

        public async Task<Client?> GetClientByIdAsync(long id)
        {
            try
            {
                var query = SqlHelper.CreateSelectByQuery(Table.Clients, Column.Client.ClientID);

                var parameters = SqlHelper.CreateParameters(
                   (Column.Client.ClientID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new Client
                {
                    Id = reader.GetInt64(Column.Client.ClientID),
                    CompanyName = reader.GetString(Column.Client.CompanyName),
                    ContactPerson = reader.GetString(Column.Client.ContactPerson),
                    PhoneNumber = reader.GetString(Column.Client.PhoneNumber),
                    Email = reader.GetString(Column.Client.Email),
                    AddressID = reader.GetInt64(Column.Client.AddressID),
                    CreatedAt = reader.GetDateTime(Column.Client.CreatedAt),
                    UpdatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(Column.Client.UpdatedAt),
                    DeletedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(Column.Client.DeletedAt),
                }, parameters);

                return result.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ClientRepository).ToString()} Get {nameof(Client)} by Id Error", typeof(ClientRepository));
                throw;
            }
        }

        public async Task<int> AddClientAsync(Client client)
        {
            try
            {
                var columns = new[]
                {
                    Column.Client.CompanyName,
                    Column.Client.ContactPerson,
                    Column.Client.PhoneNumber,
                    Column.Client.Email,
                    Column.Client.AddressID,
                };

                var query = SqlHelper.CreateInsertQuery(Table.Clients, Column.Client.ClientID, columns);

                var parameters = SqlHelper.CreateParameters(
                    (Column.Client.CompanyName, SqlDbType.NVarChar, client.CompanyName),
                    (Column.Client.ContactPerson, SqlDbType.NVarChar, client.ContactPerson),
                    (Column.Client.PhoneNumber, SqlDbType.NVarChar, client.PhoneNumber),
                    (Column.Client.Email, SqlDbType.NVarChar, client.Email),
                    (Column.Client.AddressID, SqlDbType.BigInt, client.AddressID)
                );

                return await dataConnection.ExecuteScalarAsync<int>(query, parameters);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ClientRepository).ToString()} Add {nameof(Client)} Error", typeof(ClientRepository));
                throw;
            }
        }

        public async Task<bool> UpdateClientAsync(Client client)
        {
            try
            {
                var columns = new[]
               {
                    Column.Client.CompanyName,
                    Column.Client.ContactPerson,
                    Column.Client.PhoneNumber,
                    Column.Client.Email,
                    Column.Client.AddressID,
                };

                var query = SqlHelper.CreateUpdateQuery(Table.Clients, Column.Client.ClientID, columns);

                var parameters = SqlHelper.CreateParameters(
                    (Column.Client.ClientID, SqlDbType.BigInt, client.Id),
                    (Column.Client.CompanyName, SqlDbType.NVarChar, client.CompanyName),
                    (Column.Client.ContactPerson, SqlDbType.NVarChar, client.ContactPerson),
                    (Column.Client.PhoneNumber, SqlDbType.NVarChar, client.PhoneNumber),
                    (Column.Client.Email, SqlDbType.NVarChar, client.Email),
                    (Column.Client.AddressID, SqlDbType.BigInt, client.AddressID)
                );

                int rowsAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ClientRepository).ToString()} Update {nameof(Client)} Error", typeof(ClientRepository));
                throw;
            }
        }

        public async Task<bool> DeleteClientAsync(long id)
        {
            try
            {
                var query = SqlHelper.CreateDeleteQuery(Table.Clients, Column.Client.ClientID);

                var parameters = SqlHelper.CreateParameters(
                    (Column.Client.ClientID, SqlDbType.BigInt, id)
                );

                int rowAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ClientRepository).ToString()} Delete {nameof(Client)} Error", typeof(ClientRepository));
                throw;
            }
        }

        public async Task<bool> DoesClientExist(long id)
        {
            try
            {
                var column = new[]
                {
                    Column.Client.ClientID,
                };
                var procedure = StoredProcedures.CheckClientExists;
                var parameters = SqlHelper.CreateParameters( (Column.Client.ClientID, SqlDbType.BigInt, id));
                var output = SqlHelper.CreateOutputParameter("@Exists", SqlDbType.Int);
                await dataConnection.ExecuteScalarAsync<int>(procedure.ToString(), parameters, CommandType.StoredProcedure, output);
                return (int)output.Value == 1;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ClientRepository).ToString()} Delete {nameof(Client)} Error", typeof(ClientRepository));
                throw;
            }
        }
    }
}
