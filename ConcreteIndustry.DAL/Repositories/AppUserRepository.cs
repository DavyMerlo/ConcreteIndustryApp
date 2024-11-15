using ConcreteIndustry.DAL.Constants;
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
                var query = SqlHelper.CreateSelectAllQuery(Table.AppUsers);

                return await dataConnection.ExecuteAsync(query, reader => new AppUser
                {
                    Id = reader.GetInt64(Column.AppUser.UserID),
                    FirstName = reader.GetString(Column.AppUser.FirstName),
                    LastName = reader.GetString(Column.AppUser.LastName),
                    UserName = reader.GetString(Column.AppUser.UserName),
                    Email = reader.GetString(Column.AppUser.Email),
                    HashedPassword = reader.GetString(Column.AppUser.HashedPassword),
                    Role = (Roles)Enum.Parse(typeof(Roles), reader.GetString(Column.AppUser.Role)),
                    CreatedAt = reader.GetDateTime(Column.AppUser.CreatedAt),
                    UpdatedAt = reader.IsDBNull(Column.AppUser.UpdatedAt) ? (DateTime?)null : reader.GetDateTime(Column.AppUser.UpdatedAt),
                    DeletedAt = reader.IsDBNull(Column.AppUser.DeletedAt) ? (DateTime?)null : reader.GetDateTime(Column.AppUser.DeletedAt),
                    LastLoginAt = reader.IsDBNull(Column.AppUser.LastLoginAt) ? (DateTime?)null : reader.GetDateTime(Column.AppUser.LastLoginAt),
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
                var query = SqlHelper.CreateSelectByQuery(Table.AppUsers, Column.AppUser.UserID);

                var parameters = SqlHelper.CreateParameters(
                   (Column.AppUser.UserID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(query, reader =>
                new AppUser
                {
                    Id = reader.GetInt64(Column.AppUser.UserID),
                    FirstName = reader.GetString(Column.AppUser.FirstName),
                    LastName = reader.GetString(Column.AppUser.LastName),
                    UserName = reader.GetString(Column.AppUser.UserName),
                    Email = reader.GetString(Column.AppUser.Email),
                    HashedPassword = reader.GetString(Column.AppUser.HashedPassword),
                    Role = (Roles)Enum.Parse(typeof(Roles), reader.GetString(Column.AppUser.Role)),
                    CreatedAt = reader.GetDateTime(Column.AppUser.CreatedAt),
                    UpdatedAt = reader.IsDBNull(Column.AppUser.UpdatedAt) ? (DateTime?)null : reader.GetDateTime(Column.AppUser.UpdatedAt),
                    DeletedAt = reader.IsDBNull(Column.AppUser.DeletedAt) ? (DateTime?)null : reader.GetDateTime(Column.AppUser.DeletedAt),
                    LastLoginAt = reader.IsDBNull(Column.AppUser.LastLoginAt) ? (DateTime?)null : reader.GetDateTime(Column.AppUser.LastLoginAt),
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
                var parameters = SqlHelper.CreateParameters(
                    (Column.AppUser.Email, SqlDbType.NVarChar, email)
                );

                var result = await dataConnection.ExecuteAsync(StoredProcedures.GetUserByEmail, reader =>
                new AppUser
                {
                    Id = reader.GetInt64(Column.AppUser.UserID),
                    FirstName = reader.GetString(Column.AppUser.FirstName),
                    LastName = reader.GetString(Column.AppUser.LastName),
                    UserName = reader.GetString(Column.AppUser.UserName),
                    Email = reader.GetString(Column.AppUser.Email),
                    HashedPassword = reader.GetString(Column.AppUser.HashedPassword),
                    Role = (Roles)Enum.Parse(typeof(Roles), reader.GetString(Column.AppUser.Role)),
                    CreatedAt = reader.GetDateTime(Column.AppUser.CreatedAt),
                    UpdatedAt = reader.IsDBNull(Column.AppUser.UpdatedAt) ? (DateTime?)null : reader.GetDateTime(Column.AppUser.UpdatedAt),
                    DeletedAt = reader.IsDBNull(Column.AppUser.DeletedAt) ? (DateTime?)null : reader.GetDateTime(Column.AppUser.DeletedAt),
                    LastLoginAt = reader.IsDBNull(Column.AppUser.LastLoginAt) ? (DateTime?)null : reader.GetDateTime(Column.AppUser.LastLoginAt),
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
                var parameters = SqlHelper.CreateParameters(

                        (Column.AppUser.FirstName, SqlDbType.NVarChar, appUser.FirstName),
                        (Column.AppUser.LastName, SqlDbType.NVarChar, appUser.LastName),
                        (Column.AppUser.UserName, SqlDbType.NVarChar, appUser.UserName),
                        (Column.AppUser.Email, SqlDbType.NVarChar, appUser.Email),
                        (Column.AppUser.HashedPassword, SqlDbType.NVarChar, appUser.HashedPassword),
                        (Column.AppUser.Role, SqlDbType.NVarChar, appUser.Role.ToString()),
                        (Column.AppUser.UserID, SqlDbType.BigInt, appUser.Id)
                );
                return await dataConnection.ExecuteScalarAsync<int>(StoredProcedures.RegisterUser, parameters, CommandType.StoredProcedure);
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
                    Column.AppUser.FirstName,
                    Column.AppUser.LastName,
                    Column.AppUser.UserName,
                    Column.AppUser.Email,
                    Column.AppUser.Role,
                };

                var query = SqlHelper.CreateUpdateQuery(Table.AppUsers, Column.AppUser.UserID, columns);

                var parameters = SqlHelper.CreateParameters(
                       (Column.AppUser.UserID, SqlDbType.BigInt, appUser.Id),
                       (Column.AppUser.FirstName, SqlDbType.NVarChar, appUser.FirstName),
                       (Column.AppUser.LastName, SqlDbType.NVarChar, appUser.LastName),
                       (Column.AppUser.UserName, SqlDbType.NVarChar, appUser.UserName),
                       (Column.AppUser.Email, SqlDbType.NVarChar, appUser.Email),
                       (Column.AppUser.Role, SqlDbType.NVarChar, appUser.Role.ToString())
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
                var query = SqlHelper.CreateDeleteQuery(Table.AppUsers, Column.AppUser.UserID);

                var parameters = SqlHelper.CreateParameters(
                    (Column.AppUser.UserID, SqlDbType.BigInt, id)
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
                    Column.AppUser.HashedPassword,
                };

                var query = SqlHelper.CreateUpdateQuery(Table.AppUsers, Column.AppUser.UserID, columns);

                var parameters = SqlHelper.CreateParameters(
                    (Column.AppUser.UserID, SqlDbType.BigInt, userId),
                    (Column.AppUser.HashedPassword, SqlDbType.NVarChar, password)
                );

                int rowAffected = await dataConnection.ExecuteNonQueryAsync(query, parameters);
                return rowAffected > 0;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AppUserRepository).ToString()} Update {nameof(AppUser)} Password Error", typeof(AppUserRepository));
                throw;
            }
        }

        public async Task<string?> GetPasswordHashByUserId(long userId)
        {
            try
            {
                var parameters = SqlHelper.CreateParameters(
                   (Column.AppUser.UserID, SqlDbType.BigInt, userId)
                );

                return await dataConnection.ExecuteOutputParameterAsync<string>(
                   StoredProcedures.ViewHashedPasswordByUserId,
                   parameters,
                   "HashedPassword",
                   SqlDbType.NVarChar,
                   256,
                   CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(AppUserRepository).ToString()} Update {nameof(AppUser)} Password Error", typeof(AppUserRepository));
                throw;
            }
        }
    }
}
