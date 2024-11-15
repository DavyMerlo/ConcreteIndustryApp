using ConcreteIndustry.DAL.Constants;
using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Helpers;
using ConcreteIndustry.DAL.Repositories.Helpers.Interfaces;
using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ConcreteIndustry.DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDataConnection dataConnection;
        private ILogger logger;

        public ProjectRepository(IDataConnection connection, ILogger logger)
        {
            this.dataConnection = connection;
            this.logger = logger;
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            try
            {
                return await dataConnection.ExecuteAsync(StoredProcedures.ViewProjects, reader => new Project
                {
                    Id = reader.GetInt64(Column.Project.ProjectID),
                    Name = reader.GetString(Column.Project.Name),
                    Location = reader.GetString(Column.Project.Location),
                    ClientID = reader.GetInt64(Column.Project.ClientID),
                    StartDate = reader.GetDateTime(Column.Project.StartDate),
                    EndDate = reader.GetDateTime(Column.Project.EndDate),
                    EstimatedValue = reader.GetDecimal(Column.Project.EstimatedVolume),
                    CreatedAt = reader.GetDateTime(Column.Project.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(Column.Project.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(Column.Project.DeletedAt),
                },
                null, 
                CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ProjectRepository).ToString()} Get All {nameof(Project)} Error", typeof(ProjectRepository));
                throw;
            }
        }

        public async Task<(IEnumerable<Project> Projects, int TotalCount, int TotalPages, bool HasNext, bool HasPrevious)> GetProjectsPaginatedAsync(
            int pageNumber, int pageSize)
        {
            var parameters = SqlHelper.CreateParameters(
                 (DynamicParams.Project.PageNumber, SqlDbType.Int, pageNumber),
                 (DynamicParams.Project.PageSize, SqlDbType.Int, pageSize)
            );

            var test = await dataConnection.ExecuteAsyncWithMetaData(StoredProcedures.ViewProjectsPaginated, reader => new Project
            {
                Id = reader.GetInt64(Column.Project.ProjectID),
                Name = reader.GetString(Column.Project.Name),
                Location = reader.GetString(Column.Project.Location),
                ClientID = reader.GetInt64(Column.Project.ClientID),
                StartDate = reader.GetDateTime(Column.Project.StartDate),
                EndDate = reader.GetDateTime(Column.Project.EndDate),
                EstimatedValue = reader.GetDecimal(Column.Project.EstimatedVolume),
                CreatedAt = reader.GetDateTime(Column.Project.CreatedAt),
                UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(Column.Project.UpdatedAt),
                DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(Column.Project.DeletedAt),
            },
               parameters,
               CommandType.StoredProcedure
            );

            return test;
        }

        public async Task<Project?> GetProjectByIdAsync(long id)
        {
            try
            {
                var parameters = SqlHelper.CreateParameters(
                    (Column.Project.ProjectID, SqlDbType.Int, id)
                );

                var result = await dataConnection.ExecuteAsync(StoredProcedures.ViewProjectsById, reader => new Project
                {
                    Id = reader.GetInt64(Column.Project.ProjectID),
                    Name = reader.GetString(Column.Project.Name),
                    Location = reader.GetString(Column.Project.Location),
                    ClientID = reader.GetInt64(Column.Project.ClientID),
                    StartDate = reader.GetDateTime(Column.Project.StartDate),
                    EndDate = reader.GetDateTime(Column.Project.EndDate),
                    EstimatedValue = reader.GetDecimal(Column.Project.EstimatedVolume),
                    CreatedAt = reader.GetDateTime(Column.Project.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime(Column.Project.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime(Column.Project.DeletedAt),
                }, 
                parameters, 
                CommandType.StoredProcedure
                );
                return result.SingleOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ProjectRepository).ToString()} Get {nameof(Project)} by Id Error", typeof(ProjectRepository));
                throw;
            }
        }

        public async Task<long?> AddProjectAsync(Project project)
        {
            try
            {
                var parameters = SqlHelper.CreateParameters(
                     (Column.Project.Name, SqlDbType.NVarChar, project.Name),
                     (Column.Project.Location, SqlDbType.NVarChar, project.Location),
                     (Column.Project.ClientID, SqlDbType.BigInt, project.ClientID),
                     (Column.Project.StartDate, SqlDbType.Date, project.StartDate.Date),
                     (Column.Project.EndDate, SqlDbType.Date, project.EndDate.Date),
                     (Column.Project.EstimatedVolume, SqlDbType.Decimal, project.EstimatedValue),
                     (Column.Project.ProjectID, SqlDbType.BigInt, project.Id)
                );
                return await dataConnection.ExecuteScalarAsync<int>(
                    StoredProcedures.AddProject.ToString(), 
                    parameters,
                    CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ProjectRepository).ToString()} Add {nameof(Project)} Error", typeof(ProjectRepository));
                throw;
            }
        }

        public async Task<bool> UpdateProjectAsync(Project project)
        {
            try
            {
                var parameters = SqlHelper.CreateParameters(
                     (Column.Project.ProjectID, SqlDbType.BigInt, project.Id),
                     (Column.Project.Name, SqlDbType.NVarChar, project.Name),
                     (Column.Project.Location, SqlDbType.NVarChar, project.Location),
                     (Column.Project.ClientID, SqlDbType.BigInt, project.ClientID),
                     (Column.Project.StartDate, SqlDbType.Date, project.StartDate.Date),
                     (Column.Project.EndDate, SqlDbType.Date, project.EndDate.Date),
                     (Column.Project.EstimatedVolume, SqlDbType.Decimal, project.EstimatedValue)
                );

                return await dataConnection.ExecuteNonQueryAsyncNew(
                    StoredProcedures.UpdateProject.ToString(), 
                    parameters, 
                    "IsSuccessful", 
                    CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ProjectRepository).ToString()} Update {nameof(Project)} Error", typeof(ProjectRepository));
                throw;
            }
        }

        public async Task<bool> DeleteProjectAsync(long id)
        {
            try
            {
                var parameters = SqlHelper.CreateParameters(
                    (Column.Project.ProjectID, SqlDbType.BigInt, id)
                );

                return await dataConnection.ExecuteNonQueryAsyncNew(
                    StoredProcedures.DeleteProject.ToString(),
                    parameters,
                    "IsSuccessful",
                    CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(ProjectRepository).ToString()} Delete {nameof(Project)} Error", typeof(ProjectRepository));
                throw;
            }
        }
    }
}
