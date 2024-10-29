using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.Api;
using ConcreteIndustry.BLL.DTOs.Responses.ConcreteMixes;
using ConcreteIndustry.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcreteIndustry.API.Controllers
{
    //[Authorize(Roles = "User, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ConcreteMixesController : BaseController
    {
        public ConcreteMixesController(IService service) : base(service) 
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllConcreteMixes()
        {
            var concreteMixes = await service.ConcreteMixService.GetAll();
            if (!concreteMixes.Any())
            {
                return NotFound();
            }
            var response = new ApiResponse<IEnumerable<ConcreteMixDTO>>(true, "success", concreteMixes, 200);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<IActionResult> GetConcreteMix(long id)
        {
            var concreteMix = await service.ConcreteMixService.GetById(id);
            var response = new ApiResponse<ConcreteMixDTO>(true, "success", concreteMix, 200);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddConcreteMix([FromBody] CreateConcreteMixRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var concreteMix = await service.ConcreteMixService.Add(request);
            var response = new ApiResponse<ConcreteMixDTO>(true, "success", concreteMix, 200);
            return Ok(response);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateConcreteMix([FromBody] UpdateConcreteMixRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var concreteMix = await service.ConcreteMixService.Update(request);
            var response = new ApiResponse<ConcreteMixDTO>(true, "success", concreteMix, 200);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteConcreteMix(long id)
        {
            await service.ConcreteMixService.DeleteById(id);
            var response = new ApiResponse<string>(true, "success", null, 200);
            return Ok(response);
        }
    }
}
