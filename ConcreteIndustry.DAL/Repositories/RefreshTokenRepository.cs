using ConcreteIndustry.DAL.Constants;
using ConcreteIndustry.DAL.Entities;
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
                var query = $"SELECT * FROM {Table.UserRefreshTokens} WHERE {Column.RefreshToken.DeletedAt} IS NULL " +
                    $"AND {Column.RefreshToken.Revoked} IS NULL " +
                    $"AND {Column.RefreshToken.UserID} = @{Column.RefreshToken.UserID}";

                var parameters = SqlHelper.CreateParameters(
                    (Column.RefreshToken.UserID, SqlDbType.BigInt, userId)
                );

                var result = await dataConnection.ExecuteAsync(query, reader => new RefreshToken
                {
                    Id = reader.GetInt64(Column.RefreshToken.RefreshTokenID),
                    UserID = reader.GetInt64(Column.RefreshToken.UserID),
                    RefreshTokenHash = reader.GetString(Column.RefreshToken.RefreshTokenHash),
                    Expired = reader.GetDateTime(Column.RefreshToken.Expired),
                    Revoked = reader.IsDBNull(4) ? null : reader.GetDateTime(Column.RefreshToken.Revoked),
                    CreatedAt = reader.GetDateTime(Column.RefreshToken.CreatedAt),
                    UpdatedAt = reader.IsDBNull(6) ? null : reader.GetDateTime(Column.RefreshToken.UpdatedAt),
                    DeletedAt = reader.IsDBNull(7) ? null : reader.GetDateTime(Column.RefreshToken.DeletedAt),
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
                    Column.RefreshToken.UserID,
                    Column.RefreshToken.RefreshTokenHash,
                    Column.RefreshToken.Expired,
                    Column.RefreshToken.Revoked
                };

                var query = SqlHelper.CreateInsertQuery(Table.UserRefreshTokens, Column.RefreshToken.RefreshTokenID, columns);

                var parameters = SqlHelper.CreateParameters(
                    (Column.RefreshToken.UserID, SqlDbType.BigInt, refreshToken.UserID),
                    (Column.RefreshToken.RefreshTokenHash, SqlDbType.NVarChar, refreshToken.RefreshTokenHash),
                    (Column.RefreshToken.Expired, SqlDbType.DateTime, refreshToken.Expired),
                    (Column.RefreshToken.Revoked, SqlDbType.DateTime, refreshToken.Revoked ?? (object)DBNull.Value)
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
                    Column.RefreshToken.UserID,
                    Column.RefreshToken.RefreshTokenHash,
                    Column.RefreshToken.Expired,
                    Column.RefreshToken.Revoked
                };

                var query = SqlHelper.CreateUpdateQuery(Table.UserRefreshTokens, Column.RefreshToken.RefreshTokenID, columns);

                var parameters = SqlHelper.CreateParameters(
                    (Column.RefreshToken.RefreshTokenID, SqlDbType.BigInt, refreshToken.Id),
                    (Column.RefreshToken.UserID, SqlDbType.BigInt, refreshToken.UserID),
                    (Column.RefreshToken.RefreshTokenHash, SqlDbType.NVarChar, refreshToken.RefreshTokenHash),
                    (Column.RefreshToken.Expired, SqlDbType.DateTime, refreshToken.Expired),
                    (Column.RefreshToken.Revoked, SqlDbType.DateTime, refreshToken.Revoked ?? (object)DBNull.Value)
                );

                int rowsAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(TokenRepository).ToString()} Update {nameof(RefreshToken)} Error", typeof(RefreshTokenRepository));
                throw;
            }
        }

        public async Task<bool> IsRefreshTokenValid(string refreshTokenHash)
        {
            try
            {
                var parameters = SqlHelper.CreateParameters(
                    (Column.RefreshToken.RefreshTokenHash, SqlDbType.NVarChar, refreshTokenHash)
                );

                return await dataConnection.ExecuteNonQueryAsyncNew(
                        StoredProcedures.IsValidRefreshToken.ToString(),
                        parameters,
                        "IsValid",
                        CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(TokenRepository).ToString()} Check {nameof(RefreshToken)} is valid Error", typeof(RefreshTokenRepository));
                throw;
            }
        }
    }
}
