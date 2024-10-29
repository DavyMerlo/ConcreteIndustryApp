using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Enums;
using ConcreteIndustry.DAL.Helpers;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ConcreteIndustry.DAL.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly IDataConnection dataConnection;
        private readonly ILogger logger;

        public AppUserRepository(IDataConnection dataConnection, ILogger logger)
        {
            this.dataConnection = dataConnection;
            this.logger = logger;
        }

        public async Task<IEnumerable<AppUser>> GetAppUsersAsync()
        {
            try
            {
                var query = SqlHelper<AppUserColumn>.CreateSelectAllQuery(TableName.AppUsers);

                return await dataConnection.ExecuteAsync(query, reader => new AppUser
                {
                    Id = reader.GetInt64((int)AppUserColumn.UserID),
                    FirstName = reader.GetString((int)AppUserColumn.FirstName),
                    LastName = reader.GetString((int)AppUserColumn.LastName),
                    UserName = reader.GetString((int)AppUserColumn.UserName),
                    Email = reader.GetString((int)AppUserColumn.Email),
                    HashedPassword = reader.GetString((int)AppUserColumn.HashedPassword),
                    Role = (Roles)Enum.Parse(typeof(Roles), reader.GetString((int)AppUserColumn.Role)),
                    CreatedAt = reader.GetDateTime((int)AppUserColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull((int)AppUserColumn.UpdatedAt) ? (DateTime?)null : reader.GetDateTime((int)AppUserColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull((int)AppUserColumn.DeletedAt) ? (DateTime?)null : reader.GetDateTime((int)AppUserColumn.DeletedAt),
                    LastLoginAt = reader.IsDBNull((int)AppUserColumn.LastLoginAt) ? (DateTime?)null : reader.GetDateTime((int)AppUserColumn.LastLoginAt),
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AppUserRepository).ToString()} Get All {nameof(AppUser)} Error", typeof(AppUserRepository));
                throw;
            }
        }

        public async Task<AppUser?> GetUserByIdAsync(long id)
        {
            try
            {
                var query = SqlHelper<AppUserColumn>.CreateSelectByQuery(TableName.AppUsers, AppUserColumn.UserID);

                var parameters = SqlHelper<AppUserColumn>.CreateParameters(
                   (AppUserColumn.UserID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader =>
                new AppUser
                {
                    Id = reader.GetInt64((int)AppUserColumn.UserID),
                    FirstName = reader.GetString((int)AppUserColumn.FirstName),
                    LastName = reader.GetString((int)AppUserColumn.LastName),
                    UserName = reader.GetString((int)AppUserColumn.UserName),
                    Email = reader.GetString((int)AppUserColumn.Email),
                    HashedPassword = reader.GetString((int)AppUserColumn.HashedPassword),
                    Role = (Roles)Enum.Parse(typeof(Roles), reader.GetString((int)AppUserColumn.Role)),
                    CreatedAt = reader.GetDateTime((int)AppUserColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull((int)AppUserColumn.UpdatedAt) ? (DateTime?)null : reader.GetDateTime((int)AppUserColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull((int)AppUserColumn.DeletedAt) ? (DateTime?)null : reader.GetDateTime((int)AppUserColumn.DeletedAt),
                    LastLoginAt = reader.IsDBNull((int)AppUserColumn.LastLoginAt) ? (DateTime?)null : reader.GetDateTime((int)AppUserColumn.LastLoginAt),
                }, parameters);

                return result.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AppUserRepository).ToString()} Get {nameof(AppUser)} by Id Error", typeof(AppUserRepository));
                throw;
            }
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string email)
        {
            try
            {
                var parameters = SqlHelper<AppUserColumn>.CreateParameters(
                    (AppUserColumn.Email, SqlDbType.NVarChar, email)
                );

                Enum procedure = StoredProcedures.GetUserByEmail;

                var result = await dataConnection.ExecuteAsync(procedure.ToString(), reader =>
                new AppUser
                {
                    Id = reader.GetInt64((int)AppUserColumn.UserID),
                    FirstName = reader.GetString((int)AppUserColumn.FirstName),
                    LastName = reader.GetString((int)AppUserColumn.LastName),
                    UserName = reader.GetString((int)AppUserColumn.UserName),
                    Email = reader.GetString((int)AppUserColumn.Email),
                    HashedPassword = reader.GetString((int)AppUserColumn.HashedPassword),
                    Role = (Roles)Enum.Parse(typeof(Roles), reader.GetString((int)AppUserColumn.Role)),
                    CreatedAt = reader.GetDateTime((int)AppUserColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull((int)AppUserColumn.UpdatedAt) ? (DateTime?)null : reader.GetDateTime((int)AppUserColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull((int)AppUserColumn.DeletedAt) ? (DateTime?)null : reader.GetDateTime((int)AppUserColumn.DeletedAt),
                    LastLoginAt = reader.IsDBNull((int)AppUserColumn.LastLoginAt) ? (DateTime?)null : reader.GetDateTime((int)AppUserColumn.LastLoginAt),
                }, parameters, CommandType.StoredProcedure);

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AppUserRepository).ToString()} Get {nameof(AppUser)} By Username Error", typeof(AppUserRepository));
                throw;
            }
        }

        public async Task<int> RegisterUserAsync(AppUser appUser)
        {
            try
            {
                var columns = new[]
                {
                    AppUserColumn.FirstName,
                    AppUserColumn.LastName,
                    AppUserColumn.UserName,
                    AppUserColumn.Email,
                    AppUserColumn.HashedPassword,
                    AppUserColumn.Role,
                };

                Enum procedure = StoredProcedures.RegisterUser;

                var parameters = SqlHelper<AppUserColumn>.CreateParameters(

                        (AppUserColumn.FirstName, SqlDbType.NVarChar, appUser.FirstName),
                        (AppUserColumn.LastName, SqlDbType.NVarChar, appUser.LastName),
                        (AppUserColumn.UserName, SqlDbType.NVarChar, appUser.UserName),
                        (AppUserColumn.Email, SqlDbType.NVarChar, appUser.Email),
                        (AppUserColumn.HashedPassword, SqlDbType.NVarChar, appUser.HashedPassword),
                        (AppUserColumn.Role, SqlDbType.NVarChar, appUser.Role.ToString()),
                        (AppUserColumn.UserID, SqlDbType.BigInt, appUser.Id)
                );

                var userId = await dataConnection.ExecuteScalarAsync<int>(procedure.ToString(), parameters, CommandType.StoredProcedure);
                return userId;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AppUserRepository).ToString()} Register {nameof(AppUser)} Error", typeof(AppUserRepository));
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(AppUser appUser)
        {
            try
            {
                var columns = new[]
                {
                    AppUserColumn.FirstName,
                    AppUserColumn.LastName,
                    AppUserColumn.UserName,
                    AppUserColumn.Email,
                    AppUserColumn.Role,
                };

                var query = SqlHelper<AppUserColumn>.CreateUpdateQuery(TableName.AppUsers, AppUserColumn.UserID, columns);

                var parameters = SqlHelper<AppUserColumn>.CreateParameters(
                       (AppUserColumn.UserID, SqlDbType.BigInt, appUser.Id),
                       (AppUserColumn.FirstName, SqlDbType.NVarChar, appUser.FirstName),
                       (AppUserColumn.LastName, SqlDbType.NVarChar, appUser.LastName),
                       (AppUserColumn.UserName, SqlDbType.NVarChar, appUser.UserName),
                       (AppUserColumn.Email, SqlDbType.NVarChar, appUser.Email),
                       (AppUserColumn.Role, SqlDbType.NVarChar, appUser.Role.ToString())
                );

                int rowsAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AppUserRepository).ToString()} Update {nameof(AppUser)} Error", typeof(AppUserRepository));
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            try
            {
                var query = SqlHelper<AppUserColumn>.CreateDeleteQuery(TableName.AppUsers, AppUserColumn.UserID);

                var parameters = SqlHelper<AppUserColumn>.CreateParameters(
                    (AppUserColumn.UserID, SqlDbType.BigInt, id)
                );

                int rowAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AppUserRepository).ToString()} Delete {nameof(AppUser)} Error", typeof(AppUserRepository));
                throw;
            }
        }

        public async Task<bool> UpdatePasswordAsync(long userId, string password)
        {
            try
            {
                var columns = new[]
                {
                    AppUserColumn.HashedPassword,
                };

                var query = SqlHelper<AppUserColumn>.CreateUpdateQuery(TableName.AppUsers, AppUserColumn.UserID, columns);

                var parameters = SqlHelper<AppUserColumn>.CreateParameters(
                    (AppUserColumn.UserID, SqlDbType.BigInt, userId),
                    (AppUserColumn.HashedPassword, SqlDbType.NVarChar, password)
                );

                int rowAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AppUserRepository).ToString()} Update {nameof(AppUser)}.{password} Error", typeof(AppUserRepository));
                throw;
            }
        }
    }
}
