using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Api;
using ConcreteIndustry.BLL.DTOs.Responses.Users;
using ConcreteIndustry.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConcreteIndustry.API.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly ILogger<UsersController> logger;

        public UsersController(IService service, ILogger<UsersController> logger) : base(service)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await service.AppUserService.GetAll();
            var response = new ApiResponse<IEnumerable<AppUserDTO>>(true, "succes", users, 200);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<IActionResult> GetUser(long id)
        {
            var appUser = await service.AppUserService.GetById(id);
            if (appUser == null)
            {
                return NotFound();
            }
            var response = new ApiResponse<AppUserDTO>(true, "success", appUser, 200);
            return Ok(response);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateAppUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var appUser = await service.AppUserService.Update(request);
            var response = new ApiResponse<AppUserDTO>(true, "success", appUser, 200);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProject(long id)
        {
            await service.AppUserService.DeleteById(id);
            var response = new ApiResponse<string>(true, "success", null, 200);
            return Ok(response);
        }
    }
}
