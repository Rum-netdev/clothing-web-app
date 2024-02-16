using ClothStoreApp.Data.Entities;
using ClothStoreApp.Handler.Infrastructures;
using ClothStoreApp.Handler.Products.Commands;
using ClothStoreApp.Handler.Products.Queries;
using ClothStoreApp.Handler.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ClothStoreApp.Web.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IBroker _broker;

        public ProductsController(IBroker broker)
        {
            _broker = broker;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductById([FromQuery] GetProductByIdQuery request)
        {
            var result = await _broker.Query(request);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<PaginationResult<ICollection<Product>>> GetProductsPaging([FromQuery]GetProductsPagingQuery request)
        {
            return await _broker.Query(request);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand request)
        {
            var result = await _broker.Command(request);

            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommand request)
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(DeleteProductCommand request)
        {
            var result = await _broker.Command(request);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
