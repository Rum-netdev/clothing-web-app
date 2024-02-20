using ClothStoreApp.Handler.Categories.Commands;
using ClothStoreApp.Handler.Categories.Queries;
using ClothStoreApp.Handler.Infrastructures;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public async Task<IActionResult> GetCategoriesPaging([FromQuery]GetCategoriesPagingQuery request)
        {
            var result = await _broker.Query(request);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm]CreateCategoryCommand request)
        {
            var result = await _broker.Command(request);

            if(result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryCommand request)
        {
            var result = await _broker.Command(request);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryCommand request)
        {
            var result = await _broker.Command(request);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
