using ConcreteIndustry.DAL.Constants;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Helpers;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ConcreteIndustry.DAL.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IDataConnection dataConnection;
        private ILogger logger;

        public TokenRepository(IDataConnection dataConnection, ILogger logger)
        {
            this.dataConnection = dataConnection;
            this.logger = logger;
        }

        public async Task<UserToken?> GetUserTokenByUserIdAsync(long userId)
        {
            try
            {
                var query = $"SELECT * FROM {Table.UserTokens} WHERE {Column.UserToken.DeletedAt} IS NULL " +
                    $"AND {Column.UserToken.Revoked} IS NULL " +
                    $"AND {Column.UserToken.UserID} = @{Column.UserToken.UserID}";

                var parameters = SqlHelper.CreateParameters(
                    (Column.UserToken.UserID, SqlDbType.BigInt, userId)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new UserToken
                {
                    Id = reader.GetInt64(Column.UserToken.TokenID),
                    UserID = reader.GetInt64(Column.UserToken.UserID),
                    Token = reader.GetString(Column.UserToken.Token),
                    Expired = reader.GetDateTime(Column.UserToken.Expired),
                    Revoked = reader.IsDBNull(4) ? null : reader.GetDateTime(Column.UserToken.Revoked),
                    CreatedAt = reader.GetDateTime(Column.UserToken.CreatedAt),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(Column.UserToken.UpdatedAt),
                    DeletedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(Column.UserToken.DeletedAt),
                }, parameters);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(TokenRepository).ToString()} Get {nameof(UserToken)} By UserId Error", typeof(TokenRepository));
                throw;
            }
        }

        public async Task<UserToken?> GetUserTokenByTokenAsync(string token)
        {
            try
            {
                var query = SqlHelper.CreateSelectByQuery(Table.UserTokens, Column.UserToken.Token);

                var parameters = SqlHelper.CreateParameters(
                    (Column.UserToken.Token, SqlDbType.NVarChar, token)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new UserToken
                {
                    Id = reader.GetInt64(Column.UserToken.TokenID),
                    UserID = reader.GetInt64(Column.UserToken.UserID),
                    Token = reader.GetString(Column.UserToken.Token),
                    Expired = reader.GetDateTime(Column.UserToken.Expired),
                    Revoked = reader.IsDBNull(4) ? null : reader.GetDateTime(Column.UserToken.Revoked),
                    CreatedAt = reader.GetDateTime(Column.UserToken.CreatedAt),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(Column.UserToken.UpdatedAt),
                    DeletedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(Column.UserToken.DeletedAt),
                }, parameters);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(TokenRepository).ToString()} Get {nameof(UserToken)} By Token Error", typeof(TokenRepository));
                throw;
            }
        }

        public async Task<int> AddUserTokenAsync(UserToken token)
        {
            try
            {
                var columns = new[]
                {
                    Column.UserToken.UserID,
                    Column.UserToken.Token,
                    Column.UserToken.Expired,
                    Column.UserToken.Revoked
                };

                var query = SqlHelper.CreateInsertQuery(Table.UserTokens, Column.UserToken.TokenID, columns);

                var parameters = SqlHelper.CreateParameters(
                    (Column.UserToken.UserID, SqlDbType.BigInt, token.UserID),
                    (Column.UserToken.Token, SqlDbType.NVarChar, token.Token),
                    (Column.UserToken.Expired, SqlDbType.DateTime, token.Expired),
                    (Column.UserToken.Revoked, SqlDbType.DateTime, token.Revoked ?? (object)DBNull.Value)
                );

                var newTokenId = await dataConnection.ExecuteScalarAsync<int>(query, parameters);
                return newTokenId;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(TokenRepository).ToString()} Add {nameof(UserToken)} Error", typeof(TokenRepository));
                throw;
            }
        }

        public async Task<bool> UpdateUserTokenAsync(UserToken token)
        {
            try
            {
                var columns = new[]
                {
                    Column.UserToken.UserID,
                    Column.UserToken.Token,
                    Column.UserToken.Expired,
                    Column.UserToken.Revoked
                };

                var query = SqlHelper.CreateUpdateQuery(Table.UserTokens, Column.UserToken.TokenID, columns);

                var parameters = SqlHelper.CreateParameters(
                    (Column.UserToken.TokenID, SqlDbType.BigInt, token.Id),
                    (Column.UserToken.UserID, SqlDbType.BigInt, token.UserID),
                    (Column.UserToken.Token, SqlDbType.NVarChar, token.Token),
                    (Column.UserToken.Expired, SqlDbType.DateTime, token.Expired),
                    (Column.UserToken.Revoked, SqlDbType.DateTime, token.Revoked ?? (object)DBNull.Value)
                );

                int rowsAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(TokenRepository).ToString()} Update {nameof(UserToken)} Error", typeof(TokenRepository));
                throw;
            }
        }
    }
}
