using ConcreteIndustry.BLL.DTOs.Responses.Api;
using ConcreteIndustry.BLL.DTOs.Responses.Materials;
using ConcreteIndustry.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcreteIndustry.API.Controllers
{
    //[Authorize(Roles = "User, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : BaseController
    {
        public MaterialsController(IService service) : base(service)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMaterials()
        {
            var materials = await service.MaterialService.GetAll();
            if (!materials.Any())
            {
                return NotFound();
            }
            var response = new ApiResponse<IEnumerable<MaterialDTO>>(true, "success", materials, 200);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<IActionResult> GetMaterial(long id)
        {
            var material = await service.MaterialService.GetById(id);
            var response = new ApiResponse<MaterialDTO>(true, "success", material, 200);
            return Ok(response);
        }
    }
}
