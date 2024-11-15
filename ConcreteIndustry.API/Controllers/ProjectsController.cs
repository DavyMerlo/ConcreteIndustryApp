using ConcreteIndustry.BLL.DTOs.Responses.Api;
using ConcreteIndustry.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Projects;

namespace ConcreteIndustry.API.Controllers
{
    //[Authorize(Roles = "User, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : BaseController
    {
        public ProjectsController(IService service) : base(service)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await service.ProjectService.GetAll();
            if (!projects.Any())
            {
                return NotFound();
            }
            var response = new ApiResponse<IEnumerable<ProjectDTO>>(true, "success", projects, 200);
            return Ok(response);
        }

        [HttpGet("{pageNumber:int=1}/{pageSize:int=10}")]
        public async Task<IActionResult> GetAllProjectsPaginated(int pageNumber = 1, int pageSize = 10)
        {
            var projects = await service.ProjectService.GetAllPaginated(pageNumber, pageSize);
            if (!projects.Projects.Any())
            {
                return NotFound();
            }
            var response = new PaginatedApiResponse<IEnumerable<ProjectDTO>>(
                true, 
                "success",
                projects.Projects, 
                200,
                projects.TotalCount,
                projects.TotalPages,
                pageSize,
                pageNumber,
                projects.HasNext,
                projects.HasPrevious);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<IActionResult> GetProject(long id)
        {
            var project = await service.ProjectService.GetById(id);
            var response = new ApiResponse<ProjectDTO>(true, "success", project, 200);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] CreateProjectRequest request)
        {
            var project = await service.ProjectService.Add(request);
            var response = new ApiResponse<ProjectDTO>(true, "success", project, 201);
            return CreatedAtAction(nameof(AddProject), response);
        }


        [HttpPut()]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectRequest request)
        {
            var project = await service.ProjectService.Update(request);
            var response = new ApiResponse<ProjectDTO>(true, "success", project , 200);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProject(long id)
        {
            var isDeleted = await service.ProjectService.DeleteById(id);
            var response = new ApiResponse<bool>(true, "success", isDeleted, 200);
            return Ok(response);
        }
    }
}
