using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Enums;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;
using ConcreteIndustry.DAL.Helpers;

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
                var query = SqlHelper<ClientColumn>.CreateSelectAllQuery(TableName.Clients);

                return await dataConnection.ExecuteAsync(query, reader => new Client
                {
                    Id = reader.GetInt64((int)ClientColumn.ClientID),
                    CompanyName = reader.GetString((int)ClientColumn.CompanyName),
                    ContactPerson = reader.GetString((int)ClientColumn.ContactPerson),
                    PhoneNumber = reader.GetString((int)ClientColumn.PhoneNumber),
                    Email = reader.GetString((int)ClientColumn.Email),
                    AddressID = reader.GetInt64((int)ClientColumn.AddressID),
                    CreatedAt = reader.GetDateTime((int)ClientColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime((int)ClientColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(8) ? null : reader.GetDateTime((int)ClientColumn.DeletedAt),
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
                var query = SqlHelper<ClientColumn>.CreateSelectByQuery(TableName.Clients, ClientColumn.ClientID);

                var parameters = SqlHelper<ClientColumn>.CreateParameters(
                   (ClientColumn.ClientID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new Client
                {
                    Id = reader.GetInt64((int)ClientColumn.ClientID),
                    CompanyName = reader.GetString((int)ClientColumn.CompanyName),
                    ContactPerson = reader.GetString((int)ClientColumn.ContactPerson),
                    PhoneNumber = reader.GetString((int)ClientColumn.PhoneNumber),
                    Email = reader.GetString((int)ClientColumn.Email),
                    AddressID = reader.GetInt64((int)ClientColumn.AddressID),
                    CreatedAt = reader.GetDateTime((int)ClientColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(7) ? null : reader.GetDateTime((int)ClientColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(8) ? null : reader.GetDateTime((int)ClientColumn.DeletedAt),
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
                    ClientColumn.CompanyName,
                    ClientColumn.ContactPerson,
                    ClientColumn.PhoneNumber,
                    ClientColumn.Email,
                    ClientColumn.AddressID,
                };

                var query = SqlHelper<ClientColumn>.CreateInsertQuery(TableName.Clients, ClientColumn.ClientID, columns);

                var parameters = SqlHelper<ClientColumn>.CreateParameters(
                    (ClientColumn.CompanyName, SqlDbType.NVarChar, client.CompanyName),
                    (ClientColumn.ContactPerson, SqlDbType.NVarChar, client.ContactPerson),
                    (ClientColumn.PhoneNumber, SqlDbType.NVarChar, client.PhoneNumber),
                    (ClientColumn.Email, SqlDbType.NVarChar, client.Email),
                    (ClientColumn.AddressID, SqlDbType.BigInt, client.AddressID)
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
                    ClientColumn.CompanyName,
                    ClientColumn.ContactPerson,
                    ClientColumn.PhoneNumber,
                    ClientColumn.Email,
                    ClientColumn.AddressID,
                };

                var query = SqlHelper<ClientColumn>.CreateUpdateQuery(TableName.Clients, ClientColumn.ClientID, columns);

                var parameters = SqlHelper<ClientColumn>.CreateParameters(
                    (ClientColumn.ClientID, SqlDbType.BigInt, client.Id),
                    (ClientColumn.CompanyName, SqlDbType.NVarChar, client.CompanyName),
                    (ClientColumn.ContactPerson, SqlDbType.NVarChar, client.ContactPerson),
                    (ClientColumn.PhoneNumber, SqlDbType.NVarChar, client.PhoneNumber),
                    (ClientColumn.Email, SqlDbType.NVarChar, client.Email),
                    (ClientColumn.AddressID, SqlDbType.BigInt, client.AddressID)
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
                var query = SqlHelper<ClientColumn>.CreateDeleteQuery(TableName.Clients, ClientColumn.ClientID);

                var parameters = SqlHelper<ClientColumn>.CreateParameters(
                    (ClientColumn.ClientID, SqlDbType.BigInt, id)
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
                    ClientColumn.ClientID,
                };
                Enum procedure = StoredProcedures.CheckClientExists;
                var parameters = SqlHelper<ClientColumn>.CreateParameters( (ClientColumn.ClientID, SqlDbType.BigInt, id));
                var output = SqlHelper<ClientColumn>.CreateOutputParameter("@Exists", SqlDbType.Int);
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
