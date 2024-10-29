using ConcreteIndustry.BLL.DTOs.Responses.Addresses;
using ConcreteIndustry.BLL.DTOs.Responses.Api;
using ConcreteIndustry.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcreteIndustry.API.Controllers
{
    //[Authorize(Roles = "User, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : BaseController
    {
        public AddressesController(IService service) : base(service)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            var addresses = await service.AddressService.GetAll();
            if (!addresses.Any())
            {
                return NotFound();
            }
            var response = new ApiResponse<IEnumerable<AddressDTO>>(true, "success", addresses, 200);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<IActionResult> GetAddress(long id)
        {
            var address = await service.AddressService.GetById(id);
            var response = new ApiResponse<AddressDTO>(true, "success", address, 200);
            return Ok(response);
        }
    }
}
