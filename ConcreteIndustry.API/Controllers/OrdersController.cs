using ConcreteIndustry.BLL.DTOs.Responses.Api;
using ConcreteIndustry.BLL.DTOs.Responses.Orders;
using ConcreteIndustry.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcreteIndustry.API.Controllers
{
    //[Authorize(Roles = "User, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : BaseController
    {
        public OrdersController(IService service) : base(service)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await service.OrderService.GetAll();
            if (!orders.Any())
            {
                return NotFound();
            }
            var response = new ApiResponse<IEnumerable<OrderDTO>>(true, "success", orders, 200);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<IActionResult> GetOrder(long id)
        {
            var order = await service.OrderService.GetById(id);
            var response = new ApiResponse<OrderDTO>(true, "success", order, 200);
            return Ok(response);
        }
    }
}
