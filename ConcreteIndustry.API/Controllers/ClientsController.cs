using ConcreteIndustry.BLL.DTOs.Responses.Api;
using ConcreteIndustry.BLL.DTOs.Responses.Clients;
using ConcreteIndustry.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcreteIndustry.API.Controllers
{
    //[Authorize(Roles = "User, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : BaseController
    {
        public ClientsController(IService service) : base(service)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await service.ClientService.GetAll();
            if (!clients.Any())
            {
                return NotFound();
            }
            var response = new ApiResponse<IEnumerable<ClientDTO>>(true, "succes", clients, 200);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<IActionResult> GetClient(long id)
        {
            var client = await service.ClientService.GetById(id);
            var response = new ApiResponse<ClientDTO>(true, "succes", client, 200);
            return Ok(response);
        }
    }
}
