using ClothStoreApp.Handler.Categories.Commands;
using ClothStoreApp.Handler.Infrastructures;
using Microsoft.AspNetCore.Mvc;

namespace ClothStoreApp.Web.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IBroker _broker;

        public CategoriesController(IBroker broker)
        {
            _broker = broker;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm]CreateCategoryCommand request)
        {
            var result = await _broker.Command(request);

            if(result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
