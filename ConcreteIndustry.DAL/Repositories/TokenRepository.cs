using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Enums;
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
                var query = $"SELECT * FROM {TableName.UserTokens} WHERE {TokenColumn.DeletedAt} IS NULL " +
                    $"AND {TokenColumn.Revoked} IS NULL " +
                    $"AND {TokenColumn.UserID} = @{TokenColumn.UserID}";

                var parameters = SqlHelper<TokenColumn>.CreateParameters(
                    (TokenColumn.UserID, SqlDbType.BigInt, userId)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new UserToken
                {
                    Id = reader.GetInt64((int)TokenColumn.TokenID),
                    UserID = reader.GetInt64((int)TokenColumn.UserID),
                    Token = reader.GetString((int)TokenColumn.Token),
                    Expired = reader.GetDateTime((int)TokenColumn.Expired),
                    Revoked = reader.IsDBNull(4) ? null : reader.GetDateTime((int)TokenColumn.Revoked),
                    CreatedAt = reader.GetDateTime((int)TokenColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime((int)TokenColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(7) ? null : reader.GetDateTime((int)TokenColumn.DeletedAt),
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
                var query = SqlHelper<TokenColumn>.CreateSelectByQuery(TableName.UserTokens, TokenColumn.Token);

                var parameters = SqlHelper<TokenColumn>.CreateParameters(
                    (TokenColumn.Token, SqlDbType.NVarChar, token)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new UserToken
                {
                    Id = reader.GetInt64((int)TokenColumn.TokenID),
                    UserID = reader.GetInt64((int)TokenColumn.UserID),
                    Token = reader.GetString((int)TokenColumn.Token),
                    Expired = reader.GetDateTime((int)TokenColumn.Expired),
                    Revoked = reader.IsDBNull(4) ? null : reader.GetDateTime((int)TokenColumn.Revoked),
                    CreatedAt = reader.GetDateTime((int)TokenColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime((int)TokenColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(7) ? null : reader.GetDateTime((int)TokenColumn.DeletedAt),
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
                    TokenColumn.UserID,
                    TokenColumn.Token,
                    TokenColumn.Expired,
                    TokenColumn.Revoked
                };

                var query = SqlHelper<TokenColumn>.CreateInsertQuery(TableName.UserTokens, TokenColumn.TokenID, columns);

                var parameters = SqlHelper<TokenColumn>.CreateParameters(
                    (TokenColumn.UserID, SqlDbType.BigInt, token.UserID),
                    (TokenColumn.Token, SqlDbType.NVarChar, token.Token),
                    (TokenColumn.Expired, SqlDbType.DateTime, token.Expired),
                    (TokenColumn.Revoked, SqlDbType.DateTime, token.Revoked ?? (object)DBNull.Value)
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
                    TokenColumn.UserID,
                    TokenColumn.Token,
                    TokenColumn.Expired,
                    TokenColumn.Revoked
                };

                var query = SqlHelper<TokenColumn>.CreateUpdateQuery(TableName.UserTokens, TokenColumn.TokenID, columns);

                var parameters = SqlHelper<TokenColumn>.CreateParameters(
                    (TokenColumn.TokenID, SqlDbType.BigInt, token.Id),
                    (TokenColumn.UserID, SqlDbType.BigInt, token.UserID),
                    (TokenColumn.Token, SqlDbType.NVarChar, token.Token),
                    (TokenColumn.Expired, SqlDbType.DateTime, token.Expired),
                    (TokenColumn.Revoked, SqlDbType.DateTime, token.Revoked ?? (object)DBNull.Value)
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
