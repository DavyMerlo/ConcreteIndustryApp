using ConcreteIndustry.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConcreteIndustry.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IService service;

        public BaseController(IService service)
        {
            this.service = service;
        }
    }
}
