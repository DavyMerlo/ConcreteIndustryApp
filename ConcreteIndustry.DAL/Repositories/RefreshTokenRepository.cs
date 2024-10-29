using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Enums;
using ConcreteIndustry.DAL.Helpers;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ConcreteIndustry.DAL.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IDataConnection dataConnection;
        private ILogger logger;

        public RefreshTokenRepository(IDataConnection dataConnection, ILogger logger)
        {
            this.dataConnection = dataConnection;
            this.logger = logger;
        }

        public async Task<RefreshToken?> GetRefreshTokenByUserIdAsync(long userId)
        {
            try
            {
                var query = $"SELECT * FROM {TableName.UserRefreshTokens} WHERE {RefreshTokenColumn.DeletedAt} IS NULL " +
                    $"AND {RefreshTokenColumn.Revoked} IS NULL " +
                    $"AND {RefreshTokenColumn.UserID} = @{RefreshTokenColumn.UserID}";

                var parameters = SqlHelper<RefreshTokenColumn>.CreateParameters(
                    (RefreshTokenColumn.UserID, SqlDbType.BigInt, userId)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new RefreshToken
                {
                    Id = reader.GetInt64((int)RefreshTokenColumn.RefreshTokenID),
                    UserID = reader.GetInt64((int)RefreshTokenColumn.UserID),
                    RefreshTokenHash = reader.GetString((int)RefreshTokenColumn.RefreshTokenHash),
                    Expired = reader.GetDateTime((int)RefreshTokenColumn.Expired),
                    Revoked = reader.IsDBNull(4) ? null : reader.GetDateTime((int)RefreshTokenColumn.Revoked),
                    CreatedAt = reader.GetDateTime((int)RefreshTokenColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime((int)RefreshTokenColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(7) ? null : reader.GetDateTime((int)RefreshTokenColumn.DeletedAt),
                }, parameters);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(RefreshTokenRepository).ToString()} Get {nameof(RefreshToken)} By UserId Error", typeof(RefreshTokenRepository));
                throw;
            }
        }

        public async Task<int> AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            try
            {
                var columns = new[]
                {
                    RefreshTokenColumn.UserID,
                    RefreshTokenColumn.RefreshTokenHash,
                    RefreshTokenColumn.Expired,
                    RefreshTokenColumn.Revoked
                };

                var query = SqlHelper<RefreshTokenColumn>.CreateInsertQuery(TableName.UserRefreshTokens, RefreshTokenColumn.RefreshTokenID, columns);

                var parameters = SqlHelper<RefreshTokenColumn>.CreateParameters(
                    (RefreshTokenColumn.UserID, SqlDbType.BigInt, refreshToken.UserID),
                    (RefreshTokenColumn.RefreshTokenHash, SqlDbType.NVarChar, refreshToken.RefreshTokenHash),
                    (RefreshTokenColumn.Expired, SqlDbType.DateTime, refreshToken.Expired),
                    (RefreshTokenColumn.Revoked, SqlDbType.DateTime, refreshToken.Revoked ?? (object)DBNull.Value)
                );

                var newRefreshTokenId = await dataConnection.ExecuteScalarAsync<int>(query, parameters);
                return newRefreshTokenId;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(RefreshTokenRepository).ToString()} Add {nameof(RefreshToken)} Error", typeof(RefreshTokenRepository));
                throw;
            }
        }

        public async Task<bool> UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            try
            {
                var columns = new[]
                {
                    RefreshTokenColumn.UserID,
                    RefreshTokenColumn.RefreshTokenHash,
                    RefreshTokenColumn.Expired,
                    RefreshTokenColumn.Revoked
                };

                var query = SqlHelper<RefreshTokenColumn>.CreateUpdateQuery(TableName.UserRefreshTokens, RefreshTokenColumn.RefreshTokenID, columns);

                var parameters = SqlHelper<RefreshTokenColumn>.CreateParameters(
                    (RefreshTokenColumn.RefreshTokenID, SqlDbType.BigInt, refreshToken.Id),
                    (RefreshTokenColumn.UserID, SqlDbType.BigInt, refreshToken.UserID),
                    (RefreshTokenColumn.RefreshTokenHash, SqlDbType.NVarChar, refreshToken.RefreshTokenHash),
                    (RefreshTokenColumn.Expired, SqlDbType.DateTime, refreshToken.Expired),
                    (RefreshTokenColumn.Revoked, SqlDbType.DateTime, refreshToken.Revoked ?? (object)DBNull.Value)
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
