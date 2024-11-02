using ConcreteIndustry.DAL.Entities;
using ConcreteIndustry.DAL.Enums;
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
                return await dataConnection.ExecuteAsync(StoredProcedures.ViewProjects.ToString(), reader => new Project
                {
                    Id = reader.GetInt64((int)ProjectColumn.ProjectID),
                    Name = reader.GetString((int)ProjectColumn.Name),
                    Location = reader.GetString((int)ProjectColumn.Location),
                    ClientID = reader.GetInt64((int)ProjectColumn.ClientID),
                    StartDate = reader.GetDateTime((int)ProjectColumn.StartDate),
                    EndDate = reader.GetDateTime((int)ProjectColumn.EndDate),
                    EstimatedValue = reader.GetDecimal((int)ProjectColumn.EstimatedVolume),
                    CreatedAt = reader.GetDateTime((int)ProjectColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime((int)ProjectColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime((int)ProjectColumn.DeletedAt),
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

        public async Task<Project?> GetProjectByIdAsync(long id)
        {
            try
            {
                var parameters = SqlHelper<ProjectColumn>.CreateParameters(
                   (ProjectColumn.ProjectID, SqlDbType.BigInt, id)
                );

                var result = await dataConnection.ExecuteAsync(StoredProcedures.ViewProjectsById.ToString(), reader => new Project
                {
                    Id = reader.GetInt64((int)ProjectColumn.ProjectID),
                    Name = reader.GetString((int)ProjectColumn.Name),
                    Location = reader.GetString((int)ProjectColumn.Location),
                    ClientID = reader.GetInt64((int)ProjectColumn.ClientID),
                    StartDate = reader.GetDateTime((int)ProjectColumn.StartDate),
                    EndDate = reader.GetDateTime((int)ProjectColumn.EndDate),
                    EstimatedValue = reader.GetDecimal((int)ProjectColumn.EstimatedVolume),
                    CreatedAt = reader.GetDateTime((int)ProjectColumn.CreatedAt),
                    UpdatedAt = reader.IsDBNull(8) ? null : reader.GetDateTime((int)ProjectColumn.UpdatedAt),
                    DeletedAt = reader.IsDBNull(9) ? null : reader.GetDateTime((int)ProjectColumn.DeletedAt),
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
                var parameters = SqlHelper<ProjectColumn>.CreateParameters(
                     (ProjectColumn.Name, SqlDbType.NVarChar, project.Name),
                     (ProjectColumn.Location, SqlDbType.NVarChar, project.Location),
                     (ProjectColumn.ClientID, SqlDbType.BigInt, project.ClientID),
                     (ProjectColumn.StartDate, SqlDbType.Date, project.StartDate.Date),
                     (ProjectColumn.EndDate, SqlDbType.Date, project.EndDate.Date),
                     (ProjectColumn.EstimatedVolume, SqlDbType.Decimal, project.EstimatedValue),
                     (ProjectColumn.ProjectID, SqlDbType.BigInt, project.Id)
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
                var parameters = SqlHelper<ProjectColumn>.CreateParameters(
                     (ProjectColumn.ProjectID, SqlDbType.BigInt, project.Id),
                     (ProjectColumn.Name, SqlDbType.NVarChar, project.Name),
                     (ProjectColumn.Location, SqlDbType.NVarChar, project.Location),
                     (ProjectColumn.ClientID, SqlDbType.BigInt, project.ClientID),
                     (ProjectColumn.StartDate, SqlDbType.Date, project.StartDate.Date),
                     (ProjectColumn.EndDate, SqlDbType.Date, project.EndDate.Date),
                     (ProjectColumn.EstimatedVolume, SqlDbType.Decimal, project.EstimatedValue)
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
                var parameters = SqlHelper<ProjectColumn>.CreateParameters(
                    (ProjectColumn.ProjectID, SqlDbType.BigInt, id)
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
